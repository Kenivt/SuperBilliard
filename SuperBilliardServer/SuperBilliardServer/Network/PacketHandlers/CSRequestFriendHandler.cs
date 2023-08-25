using GameMessage;
using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public partial class CSRequestFriendHandler : PacketAsyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSRequestFriend;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            var csPacket = (CSRequestFriend)packet;

            if (client.Player == null)
            {
                Log.Error("client.Player is null");
                return;
            }

            string userName = csPacket.OwnUsername;
            string targetUserName = csPacket.TargetUsername;

            if (userName != client.Player.UserName)
            {
                Log.Error("userName != client.Player.UserName");
                return;
            }

            var sqlResult = await SqlManager.Instance.GetSqlHandler<IFriendSqlHandler>().
                                    RequestFriend(csPacket.OwnUsername, csPacket.TargetUsername);
            SCRequestFriend scPacket = SCRequestFriend.Create();
            scPacket.TargetUserName = targetUserName;
            if (sqlResult == SqlResult.Success)
            {
                scPacket.Result = FriendHandleResult.Success;
            }
            else
            {
                scPacket.Result = FriendHandleResult.RepeatAddingFriends;
            }
            client.SendPacket(scPacket);
            ReferencePool.Release(scPacket);
        }
    }
}
