using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class CSGetPlayerMessage : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSGetPlayerMessage;

        public override void Clear()
        {
            gpmUsername_ = "default";
        }
        public static CSGetPlayerMessage Create(string username)
        {
            CSGetPlayerMessage packet = ReferencePool.Acquire<CSGetPlayerMessage>();
            packet.gpmUsername_ = username;
            return packet;
        }
    }
}