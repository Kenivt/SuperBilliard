using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSHeartBeat : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.HeartBeat;

        public override void Clear()
        {
            heartBeat_ = -1;
        }
    }
}
