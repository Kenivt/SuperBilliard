using GameFramework;
using SuperBilliardServer.Network.Packets;
namespace GameMessage
{
    public partial class CSSetBilliardState : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSSetBilliardState;
        public override void Clear()
        {
            BilliardId = -1;
            Active = false;
            physicsIsOpen_ = false;
        }
        public static CSSetBilliardState Create(int billiardId, bool active, bool physicsIsOpen = false)
        {
            CSSetBilliardState pack = ReferencePool.Acquire<CSSetBilliardState>();
            pack.BilliardId = billiardId;
            pack.Active = active;
            pack.physicsIsOpen_ = physicsIsOpen;
            return pack;
        }
    }
}
