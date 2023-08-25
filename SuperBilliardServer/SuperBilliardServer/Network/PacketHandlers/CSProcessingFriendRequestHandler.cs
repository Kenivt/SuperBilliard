using GameMessage;
using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSProcessingFriendRequestHandler : PacketAsyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSProcessingFriendRequest;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            CSProcessingFriendRequest csPacket = (CSProcessingFriendRequest)packet;

            if (client.Player == null)
            {
                Debug.Log.Error("client.Player is null");
                return;
            }
            string requesterUserName = csPacket.RequesterUserName;
            string username = client.Player.UserName;
            if (client.Player.UserName != username)
            {
                Debug.Log.Error("userName != client.Player.UserName");
                return;
            }
            var sqlResult = await SqlManager.Instance.GetSqlHandler<IFriendSqlHandler>().ProcessingFriendRequest(csPacket.OwnUsername, csPacket.RequesterUserName, csPacket.RequestState);

            SCProcessingFriendRequest scPacket = SCProcessingFriendRequest.Create();
            scPacket.RequesterUserName = requesterUserName;
            if (sqlResult == SqlResult.Success)
            {
                //如果是同意申请的话
                if (csPacket.RequestState == FriendRequestState.Agreen)
                {
                    var reqPlayer = PlayerManager.Instance.GetPlayer(csPacket.RequesterUserName);
                    //如果角色在线
                    if (reqPlayer != null)
                    {
                        //把对方添加到自身好友列表
                        reqPlayer.AddOnlineFriend(client.Player.UserName);
                        client.Player.AddOnlineFriend(reqPlayer.UserName);

                        //发送更新对应的玩家列表,这里强制查找了所有的好友列表...有待优化
                        SCFriendList sCFriendList = SCFriendList.Create();
                        await SqlManager.Instance.GetSqlHandler<IFriendSqlHandler>().GetFriendList(reqPlayer.UserName, sCFriendList);
                        foreach (var item in sCFriendList.FriendMessages)
                        {
                            if (item.IsLogin)
                            {
                                var friend = PlayerManager.Instance.GetPlayer(item.Username);
                                item.Status = friend.State;
                            }
                            else
                            {
                                item.Status = PlayerStatus.PlayerStausNone;
                            }
                        }
                        reqPlayer.SendPacket(sCFriendList);
                        ReferencePool.Release(sCFriendList);
                    }

                    //更新自己的好友列表
                    SCFriendList sCFriendList2 = SCFriendList.Create();
                    await SqlManager.Instance.GetSqlHandler<IFriendSqlHandler>().GetFriendList(client.Player.UserName, sCFriendList2);

                    foreach (var item in sCFriendList2.FriendMessages)
                    {
                        if (item.IsLogin)
                        {
                            var friend = PlayerManager.Instance.GetPlayer(item.Username);
                            item.Status = friend.State;
                        }
                        else
                        {
                            item.Status = PlayerStatus.PlayerStausNone;
                        }
                    }
                    client.Player.SendPacket(sCFriendList2);
                    ReferencePool.Release(sCFriendList2);
                }
                scPacket.Result = FriendHandleResult.Success;
            }
            else
            {
                scPacket.Result = FriendHandleResult.Failure;
            }
            client.SendPacket(scPacket);
            ReferencePool.Release(scPacket);
        }
    }
}