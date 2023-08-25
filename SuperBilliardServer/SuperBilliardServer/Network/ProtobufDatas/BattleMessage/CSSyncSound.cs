using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSSyncSound : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSSyncSound;

        public override void Clear()
        {
            SoundId = 0;
            Volumn = 1;
            Position = null;
        }
    }
}
