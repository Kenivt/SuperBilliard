using GameMessage;
using Microsoft.Data.SqlClient;
using SuperBilliardServer.Debug;

namespace SuperBilliardServer.Sql
{
    public interface IPlayerMessageSqlHandler : ISqlHandler
    {
        Task<bool> GetMessageAsync(string username, SCPlayerMessage packet);
        Task Save(CSSavePlayerMessage packet);
    }

    public class PlayerMessageSqlHandler : SqlHandlerBase, IPlayerMessageSqlHandler
    {
        public const string saveSql = "UPDATE PlayerMessage SET p_nickname = @nickName,p_hairId = @hairId," +
                                      "p_bodyId = @bodyId,p_faceId = @faceId,p_kitId = @kitId," +
                                      "p_description = @description WHERE p_userName = @username;";

        public const string getPlayerMessageSql = "SELECT p_level, p_iconId, p_nickname, p_description,p_faceId,p_hairId,p_bodyId,p_kitId " +
            "FROM PlayerLevel " +
            "INNER JOIN PlayerMessage ON PlayerLevel.p_userName = PlayerMessage.p_userName " +
            "WHERE PlayerLevel.p_userName = @userName";

        public async Task Save(CSSavePlayerMessage packet)
        {
            var controlle = SqlManager.Instance.GetConnection();
            SqlConnection sqlConnection = controlle.Connection;
            using (SqlCommand command = new SqlCommand(saveSql, sqlConnection))
            {
                command.Parameters.AddWithValue("@nickName", packet.SnikName);
                command.Parameters.AddWithValue("@hairId", packet.HairId);
                command.Parameters.AddWithValue("@bodyId", packet.BodyId);
                command.Parameters.AddWithValue("@faceId", packet.FaceId);
                command.Parameters.AddWithValue("@kitId", packet.KitId);
                command.Parameters.AddWithValue("@description", packet.Description);
                command.Parameters.AddWithValue("@username", packet.UserName);
                await command.ExecuteReaderAsync();
            }
            SqlManager.Instance.ReleaseConnection(controlle);
        }

        public async Task<bool> GetMessageAsync(string username, SCPlayerMessage packet)
        {
            if (string.IsNullOrEmpty(username))
            {
                Log.Error("username is null or empty on {0}.", this.GetType().ToString());
                return false;
            }
            SqlConnectionController connectionController = SqlManager.Instance.GetConnection();

            SqlConnection connection = connectionController.Connection;

            using (SqlCommand command = new SqlCommand(getPlayerMessageSql, connection))
            {
                command.Parameters.AddWithValue("@userName", username);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                bool flag = false;
                if (reader.Read())
                {
                    packet.PmUserName = username;
                    packet.PmLevel = reader.GetInt16(0);
                    packet.PmIconId = reader.GetInt16(1);
                    packet.PmSnikName = reader.GetString(2);
                    packet.PmDescription = reader.GetString(3);
                    packet.PmFaceId = reader.GetInt16(4);
                    packet.PmHairId = reader.GetInt16(5);
                    packet.PmBodyId = reader.GetInt16(6);
                    packet.PmKitId = reader.GetInt16(7);
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                SqlManager.Instance.ReleaseConnection(connectionController);
                reader.Dispose();
                return flag;
            }
        }
    }
}
