using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSLoadGameCompletePacketHandler : PacketSyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSLoadGameComplete;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSLoadGameComplete cSLoadGameComplete = packet as CSLoadGameComplete;

            if (cSLoadGameComplete == null)
            {
                return;
            }

            if (client.Player == null)
            {
                return;
            }
            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Error("Player is not in game room, can't load game complete.client id is{0}", client.ClientId);
                return;
            }
            gameRoom.LoadGameComplete(client.Player);
        }
    }
}
