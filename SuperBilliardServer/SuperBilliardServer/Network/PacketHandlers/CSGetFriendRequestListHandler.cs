using GameMessage;
using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Debug;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSGetFriendRequestListHandler : PacketAsyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSGetFriendRequestList;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            CSGetFriendRequestList csPacket = (CSGetFriendRequestList)packet;

            Log.Debug("CSGetFriendRequestListHandler");
            if (client.Player == null)
            {
                SuperBilliardServer.Debug.Log.Error("client.Player is null");
                return;
            }
            if (client.Player.UserName != csPacket.UserName)
            {
                SuperBilliardServer.Debug.Log.Error("client.Player.UserName != csPacket.UserName");
                return;
            }

            SCFriendRequestList sCFriendListMessage = SCFriendRequestList.Create();
            var returnPacket = await SqlManager.Instance.GetSqlHandler<IFriendSqlHandler>().GetFriendRequestList(csPacket.UserName, sCFriendListMessage);

            if (returnPacket == SqlResult.Success)
            {
                client.SendPacket(sCFriendListMessage);
            }

            ReferencePool.Release(sCFriendListMessage);
        }
    }
}
