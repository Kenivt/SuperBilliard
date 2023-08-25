using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCReturnMessage : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCReturnMessage;

        public override void Clear()
        {
            message_ = "Default";
        }
        public static SCReturnMessage Create()
        {
            SCReturnMessage scReturnMessage = ReferencePool.Acquire<SCReturnMessage>();
            return scReturnMessage;
        }
    }
}
