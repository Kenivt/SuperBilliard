using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class ReciveHeartBeatHandle : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.HeartBeat;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            SCConfirm sCConfirm = SCConfirm.Create();
            client.ResetClientInfo();
            client.SerilizePacketToMessages(sCConfirm);
            client.Send();
            ReferencePool.Release(sCConfirm);
        }
    }
}
