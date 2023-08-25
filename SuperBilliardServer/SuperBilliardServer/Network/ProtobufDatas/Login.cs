using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSLogin : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.Login;

        public override void Clear()
        {
            username_ = string.Empty;
            password_ = string.Empty;
        }
    }
    public partial class SCLogin : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.Login;

        public override void Clear()
        {
            result_ = ReturnResult.Failure;
            username_ = string.Empty;
        }
    }
}
