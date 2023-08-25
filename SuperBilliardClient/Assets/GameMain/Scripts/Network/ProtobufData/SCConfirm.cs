using SuperBilliard;
namespace GameMessage
{
    public partial class SCConfirm : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.Confirm;

        public override void Clear()
        {
            confirm_ = -1;
        }
    }

}