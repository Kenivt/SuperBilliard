using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSExitGameRoomHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSExitGameRoom;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSExitGameRoom cSExitGameRoom = packet as CSExitGameRoom;
            if (client.Player == null)
            {
                Log.Error("client.Player is null");
                return;
            }
            IGameRoom gameRoom = client.Player.GameRoom;
            gameRoom.Leave(client.Player);
            client.Player.GameRoom = null;
        }
    }
}
