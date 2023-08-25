using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCConfirm : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.Confirm;

        public override void Clear()
        {
            confirm_ = -1;
        }
        public static SCConfirm Create()
        {
            SCConfirm sCConfirm = ReferencePool.Acquire<SCConfirm>();
            return sCConfirm;
        }
    }
}
