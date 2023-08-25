using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCSetBilliardState : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCSetBilliardState;

        public override void Clear()
        {
            BilliardId = -1;
            Active = false;
            physicsIsOpen_ = false;
        }

        public static SCSetBilliardState Create(int billiardId, bool active, bool physicsIsOpen)
        {
            SCSetBilliardState pack = ReferencePool.Acquire<SCSetBilliardState>();
            pack.BilliardId = billiardId;
            pack.Active = active;
            pack.physicsIsOpen_ = physicsIsOpen;
            return pack;
        }

    }
}
