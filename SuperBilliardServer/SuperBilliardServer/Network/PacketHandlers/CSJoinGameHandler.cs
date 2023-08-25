using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSJoinGameHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSJoinGame;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSJoinGame cSJoinGame = packet as CSJoinGame;

            if (cSJoinGame == null)
            {
                return;
            }

            if (client.Player == null)
            {
                Log.Error("Player is null, can't join game room.client id is{0}", client.ClientId);
                return;
            }
            client.Player.State = PlayerStatus.Matching;
            Log.Debug("玩家加入游戏:{0}", cSJoinGame.GameType);
            GameManager.Instance.MatchGame(client.Player, cSJoinGame.GameType);
        }
    }
}