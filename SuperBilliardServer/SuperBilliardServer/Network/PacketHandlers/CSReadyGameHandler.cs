using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSReadyGameHandler : PacketSyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSReadyGame;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSReadyGame csReady = (CSReadyGame)packet;

            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Error("GameRoom is invalid. When handling the readyGame packet");
                return;
            }

            gameRoom.ReadyGame(client.Player, csReady.IsReady);

            SCReadyGame scReady = SCReadyGame.Create(csReady.Username, csReady.IsReady);
            gameRoom.Broadcast(scReady);
            Log.Debug($"玩家{csReady.Username}准备状态:{csReady.IsReady}");
            ReferencePool.Release(scReady);
        }
    }
}
