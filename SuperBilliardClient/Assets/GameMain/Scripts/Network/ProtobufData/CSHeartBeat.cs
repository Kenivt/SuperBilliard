using SuperBilliard;

namespace GameMessage
{
    public partial class CSHeartBeat : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.HeartBeat;

        public override void Clear()
        {
            heartBeat_ = -1;
        }
    }
}
