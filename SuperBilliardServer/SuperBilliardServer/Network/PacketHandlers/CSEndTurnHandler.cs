using GameFramework;
using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSEndTurnHandler : PacketSyncHandler
    {

        public override int Id => Constant.PacketTypeId.CSEndTurn;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSEndTurn cSEndTurn = packet as CSEndTurn;

            if (cSEndTurn == null)
            {
                return;
            }
            if (client.Player == null)
            {
                Log.Error("client.Player is null");
                return;
            }
            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Error("client.Player.GameRoom is null");
                return;
            }
            SCStartTurn sCStartTurn = SCStartTurn.Create(cSEndTurn.Isfoul);

            lock (gameRoom)
            {
                gameRoom.SendMessageToAnother(client.Player, sCStartTurn);
            }
            ReferencePool.Release(sCStartTurn);
        }
    }
}
