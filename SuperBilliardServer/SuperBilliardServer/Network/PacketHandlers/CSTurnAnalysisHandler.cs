using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSTurnAnalysisHandler : PacketSyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSTurnAnalysis;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSTurnAnalysis cSTurnAnalysis = (CSTurnAnalysis)packet;
            if (client.Player == null)
            {
                return;
            }
            if (client.Player.GameRoom == null)
            {
                return;
            }
            var pac = SCTurnAnalysis.Create(cSTurnAnalysis.FristCollideBIlliardId);
            //广播一下
            client.Player.GameRoom.Broadcast(pac);
            ReferencePool.Release(pac);
        }
    }
}
