using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSEndTurn : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSEndTurn;

        public override void Clear()
        {
            isfoul_ = false;
        }
    }
}
