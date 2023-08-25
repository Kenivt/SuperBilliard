using GameMessage;
using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSGetFriendListHandler : PacketAsyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSGetFriendMessage;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            var csPacket = (CSGetFriendList)packet;

            if (client.Player == null)
            {
                Log.Error("client.Player is null");
                return;
            }

            string userName = csPacket.Username;
            if (userName != client.Player.UserName)
            {
                Log.Error("userName != client.Player.UserName");
                return;
            }

            SCFriendList scPacket = ReferencePool.Acquire<SCFriendList>();
            var returnPacket = await SqlManager.Instance.GetSqlHandler<IFriendSqlHandler>().GetFriendList(userName, scPacket);

            if (returnPacket == SqlResult.Success)
            {
                var scUpdate = SCUpdateFriendState.Create(client.Player.UserName, client.Player.State);
                //获取玩家的状态
                var list = scPacket.FriendMessages;

                foreach (var item in list)
                {
                    if (item.IsLogin)
                    {
                        var friend = PlayerManager.Instance.GetPlayer(item.Username);
                        if (friend != null)
                        {
                            item.Status = friend.State;
                            //如果是登陆状态的话则添加
                            client.Player.AddOnlineFriend(item.Username);
                            friend.AddOnlineFriend(client.Player.UserName);
                            //给对应的玩家发送更新当前角色状态通知
                            friend.SendPacket(scUpdate);
                        }
                        else
                        {
                            Log.Error("错误,没有找到对应Id为{0}的玩家..", item.Username);
                        }
                    }
                    else
                    {
                        item.Status = PlayerStatus.PlayerStausNone;
                    }
                }
                client.SendPacket(scPacket);
                ReferencePool.Release(scUpdate);
            }

            //回收
            ReferencePool.Release(scPacket);
        }
    }
}