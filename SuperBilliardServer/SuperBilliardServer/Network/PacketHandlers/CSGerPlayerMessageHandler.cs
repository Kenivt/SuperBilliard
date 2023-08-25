using GameMessage;
using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSGerPlayerMessageHandler : PacketAsyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSGetPlayerMessage;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            CSGetPlayerMessage cSGetPlayerMessage = packet as CSGetPlayerMessage;

            if (client.Player == null)
            {
                Log.Error("client.Player is null");
                return;
            }
            string userName = cSGetPlayerMessage.GpmUsername;
            SCPlayerMessage sCPlayerMessage = ReferencePool.Acquire<SCPlayerMessage>();

            bool flag = await SqlManager.Instance.GetSqlHandler<IPlayerMessageSqlHandler>().GetMessageAsync(userName, sCPlayerMessage);

            if (flag)
            {

                client.SendPacket(sCPlayerMessage);
            }
            ReferencePool.Release(sCPlayerMessage);
        }
    }
}
