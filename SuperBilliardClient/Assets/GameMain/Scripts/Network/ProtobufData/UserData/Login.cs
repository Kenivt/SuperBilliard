using SuperBilliard;

namespace GameMessage
{
    public partial class CSLogin : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.Login;

        public override void Clear()
        {
            username_ = "default";
            password_ = "default";
        }
    }
    public partial class SCLogin : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.Login;

        public override void Clear()
        {
            result_ = ReturnResult.Failure;
            username_ = "default";
        }
    }
}
