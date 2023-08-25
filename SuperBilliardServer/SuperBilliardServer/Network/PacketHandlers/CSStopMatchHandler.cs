using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;
using GameFramework;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSStopMatchHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSStopMatch;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSStopMatch cSStopMatch = packet as CSStopMatch;
            if (cSStopMatch == null)
            {
                Log.Error("StopMatch 为 null..");
                return;
            }
            if (client.Player == null)
            {
                Log.Error("为什么该客户端没有对应的玩家呢？{0}", client.ClientId);
                return;
            }
            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Warning("为什么该玩家的GameRoom为null呢?{0}", client.Player.UserName);
                return;
            }
            client.Player.State = PlayerStatus.Idle;
            gameRoom.Leave(client.Player);
        }
    }
}
