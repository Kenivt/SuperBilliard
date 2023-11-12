using GameMessage;
using Microsoft.Data.SqlClient;

namespace SuperBilliardServer.Sql
{
    /// <summary>
    /// 请不要申明任何非静态属性字段
    /// </summary>
    public interface IFriendSqlHandler : ISqlHandler
    {
        Task<SqlResult> GetFriendList(string userName, SCFriendList packet);

        Task<SqlResult> GetFriendRequestList(string userName, SCFriendRequestList packet);

        Task<SqlResult> RequestFriend(string userName, string targetUserName);

        Task<SqlResult> ProcessingFriendRequest(string userName, string targetUserName, FriendRequestState state);
    }

    public class FriendSqlHandler : IFriendSqlHandler
    {
        public async Task<SqlResult> GetFriendList(string userName, SCFriendList packet)
        {
            const string getFriendListSql = @"SELECT username,nickName,description,bodyId,faceId,hairId,kitId,isLogin FROM vPlayerMessageAndLoginState INNER JOIN FriendList 
                                            ON (FriendList.p_user1 = @userName AND FriendList.p_user2 = username) OR (FriendList.p_user2 = @userName AND FriendList.p_user1 = username)";

            SqlConnectionController connectionController = SqlManager.Instance.GetConnection();
            SqlResult sqlResult = SqlResult.None;

            using (SqlCommand command = new SqlCommand(getFriendListSql, connectionController.Connection))
            {
                command.Parameters.AddWithValue("@userName", userName);

                using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(true))
                {
                    while (await reader.ReadAsync().ConfigureAwait(true))
                    {
                        PlayerMessage playerMessage = PlayerMessage.Create();
                        playerMessage.Username = reader.GetString(0);
                        playerMessage.NickName = reader.GetString(1);
                        playerMessage.Description = reader.GetString(2);
                        playerMessage.BodyId = reader.GetInt16(3);
                        playerMessage.FaceId = reader.GetInt16(4);
                        playerMessage.HairId = reader.GetInt16(5);
                        playerMessage.KitId = reader.GetInt16(6);
                        playerMessage.IsLogin = reader.GetBoolean(7);

                        packet.FriendMessages.Add(playerMessage);
                    }

                    sqlResult = SqlResult.Success;
                    if (packet.FriendMessages.Count == 0)
                    {
                        packet.Result = FriendHandleResult.None;
                    }
                    else
                    {
                        packet.Result = FriendHandleResult.Success;
                    }
                }
            }

            SqlManager.Instance.ReleaseConnection(connectionController);
            return sqlResult;
        }

        public async Task<SqlResult> GetFriendRequestList(string userName, SCFriendRequestList packet)
        {
            SqlConnectionController connectionController = SqlManager.Instance.GetConnection();
            SqlResult sqlResult = SqlResult.None;

            const string getFriendRequestListSql = "SELECT senderUserName,p_nickname,p_description,p_faceId,p_hairId,p_bodyId,p_kitId FROM FriendRequest," +
                                                    "PlayerMessage WHERE (FriendRequest.senderUserName = PlayerMessage.p_userName AND FriendRequest.recvUserName = @userName)";

            using (SqlCommand command = new SqlCommand(getFriendRequestListSql, connectionController.Connection))
            {
                command.Parameters.AddWithValue("@userName", userName);

                var checkReader = await command.ExecuteReaderAsync().ConfigureAwait(true);

                while (await checkReader.ReadAsync().ConfigureAwait(true))
                {
                    PlayerMessage playerMessage = PlayerMessage.Create();
                    playerMessage.Username = checkReader.GetString(0);
                    playerMessage.NickName = checkReader.GetString(1);
                    playerMessage.Description = checkReader.GetString(2);
                    playerMessage.FaceId = checkReader.GetInt16(3);
                    playerMessage.HairId = checkReader.GetInt16(4);
                    playerMessage.BodyId = checkReader.GetInt16(5);
                    playerMessage.KitId = checkReader.GetInt16(6);
                    packet.RequestFriendList.Add(playerMessage);
                }
                sqlResult = SqlResult.Success;
                //这是可以为0的
                if (packet.RequestFriendList.Count == 0)
                {
                    packet.Result = FriendHandleResult.None;
                }
                else
                {
                    packet.Result = FriendHandleResult.Success;
                }
            }
            SqlManager.Instance.ReleaseConnection(connectionController);
            return sqlResult;
        }

        public async Task<SqlResult> RequestFriend(string userName, string targetUserName)
        {
            const string insertSql = "INSERT INTO FriendRequest (senderUserName,recvUserName,requestTime,Status) VALUES (@sender, @reciever,GetDate(),0)";

            const string checkHasFriendSql = "SELECT COUNT(*) FROM FriendList WHERE (FriendList.p_user1 = @username AND FriendList.p_user2 = @friend) OR (FriendList.p_user1 = @friend AND FriendList.p_user2 = @username)";

            const string checkHasAwaitSql = "SELECT COUNT(*) FROM FriendRequest WHERE (FriendRequest.recvUserName = @sender AND FriendRequest.senderUserName = @reciever) OR (FriendRequest.recvUserName =  @sender AND FriendRequest.senderUserName  = @reciever) AND FriendRequest.Status = 0";

            SqlConnectionController connectionController = SqlManager.Instance.GetConnection();

            SqlResult sqlResult = SqlResult.None;

            using (SqlCommand checkFriendListCommand = new SqlCommand(checkHasFriendSql, connectionController.Connection))
            {
                checkFriendListCommand.Parameters.AddWithValue("@username", userName);
                checkFriendListCommand.Parameters.AddWithValue("@friend", targetUserName);

                using (SqlDataReader reader = await checkFriendListCommand.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        int count = reader.GetInt32(0);
                        if (count >= 1)
                        {
                            SqlManager.Instance.ReleaseConnection(connectionController);
                            return SqlResult.Failure;
                        }
                    }
                    else
                    {
                        SqlManager.Instance.ReleaseConnection(connectionController);
                        return SqlResult.Failure;
                    }
                }
            }

            using (SqlCommand command = new SqlCommand(checkHasAwaitSql, connectionController.Connection))
            {
                command.Parameters.AddWithValue("@sender", userName);
                command.Parameters.AddWithValue("@reciever", targetUserName);

                var checkReader = await command.ExecuteReaderAsync();

                if (await checkReader.ReadAsync())
                {
                    int count = checkReader.GetInt32(0);
                    checkReader.Close();
                    checkReader.Dispose();
                    //此时判断出了并没有正在处理中的好友请求
                    if (count == 0)
                    {
                        //向表中插入一条好友请求
                        using (SqlCommand insertCommand = new SqlCommand(insertSql, connectionController.Connection))
                        {
                            insertCommand.Parameters.AddWithValue("@sender", userName);
                            insertCommand.Parameters.AddWithValue("@reciever", targetUserName);

                            await insertCommand.ExecuteNonQueryAsync();
                        }
                        sqlResult = SqlResult.Success;
                    }
                    else
                    {
                        sqlResult = SqlResult.Failure;
                    }
                }
                else
                {
                    checkReader.Close();
                    checkReader.Dispose();
                    sqlResult = SqlResult.Failure;
                }
            }

            SqlManager.Instance.ReleaseConnection(connectionController);
            return sqlResult;
        }

        public async Task<SqlResult> ProcessingFriendRequest(string userName, string targetUserName, FriendRequestState state)
        {
            SqlConnectionController connectionController = SqlManager.Instance.GetConnection();

            SqlResult sqlResult = SqlResult.None;


            const string processingSql = "UPDATE FriendRequest SET Status = @state WHERE FriendRequest.senderUserName = @requesterName AND FriendRequest.recvUserName = @userName";

            const string deleteSql = "DELETE FROM FriendRequest WHERE FriendRequest.senderUserName = @requesterName AND FriendRequest.recvUserName = @userName";

            const string insertFriendSql = "INSERT INTO FriendList (p_user1, p_user2) VALUES (@userName, @targetUserName)";

            if (state == FriendRequestState.Await)
            {
                sqlResult = SqlResult.Failure;
                return sqlResult;
            }

            using (SqlCommand command = new SqlCommand(processingSql, connectionController.Connection))
            {
                command.Parameters.AddWithValue("@state", (int)state);
                command.Parameters.AddWithValue("@userName", userName);
                command.Parameters.AddWithValue("@requesterName", targetUserName);

                int result = await command.ExecuteNonQueryAsync();
                if (result == 1)
                {
                    //如果是同意好友请求,删除对应的表中数据并且向好友表中插入一条数据
                    if (state == FriendRequestState.Agreen)
                    {
                        using (SqlCommand insertCommand = new SqlCommand(insertFriendSql, connectionController.Connection))
                        {
                            insertCommand.Parameters.AddWithValue("@userName", userName);
                            insertCommand.Parameters.AddWithValue("@targetUserName", targetUserName);

                            int resultAdd = await insertCommand.ExecuteNonQueryAsync();
                            if (resultAdd == 1)
                            {
                                sqlResult = SqlResult.Success;
                            }
                            else
                            {
                                sqlResult = SqlResult.Failure;
                            }
                        }
                        using (SqlCommand deleteCommand = new SqlCommand(deleteSql, connectionController.Connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@userName", userName);
                            deleteCommand.Parameters.AddWithValue("@requesterName", targetUserName);
                            await deleteCommand.ExecuteNonQueryAsync().ConfigureAwait(true);
                        }
                    }
                    else if (state == FriendRequestState.Refuse)
                    {
                        //保留好友请求数据，等对方确认后再删除，或者过期删除
                        sqlResult = SqlResult.Success;
                    }
                }
                else
                {
                    sqlResult = SqlResult.Failure;
                }
            }
            SqlManager.Instance.ReleaseConnection(connectionController);
            return sqlResult;
        }
    }
}