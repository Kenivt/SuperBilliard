using GameFramework;
using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSGameResultHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.GameResult;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSGameResultPack cSGameResultPack = packet as CSGameResultPack;
            if (cSGameResultPack == null)
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
            gameRoom.EndGame(cSGameResultPack.Result, client.Player);
        }
    }
}
