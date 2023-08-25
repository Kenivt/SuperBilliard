using GameMessage;
using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSSavePlayerMessageHandler : PacketAsyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSSavePlayerMessage;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            CSSavePlayerMessage cSSavePlayerMessage = (CSSavePlayerMessage)packet;
            if (client.Player == null)
            {
                return;
            }
            if (client.Player.UserName != cSSavePlayerMessage.UserName)
            {
                Log.Error("client.Player.UserName != cSSavePlayerMessage.UserName");
                return;
            }
            await SqlManager.Instance.GetSqlHandler<IPlayerMessageSqlHandler>().Save(cSSavePlayerMessage);
        }
    }
}
