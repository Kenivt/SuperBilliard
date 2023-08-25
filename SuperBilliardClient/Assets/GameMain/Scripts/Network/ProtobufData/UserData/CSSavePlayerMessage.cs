using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class CSSavePlayerMessage : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSSavePlayerMessage;

        public override void Clear()
        {

        }

        public static CSSavePlayerMessage Create()
        {
            CSSavePlayerMessage cSSavePlayerMessage = ReferencePool.Acquire<CSSavePlayerMessage>();
            return cSSavePlayerMessage;
        }
    }
}