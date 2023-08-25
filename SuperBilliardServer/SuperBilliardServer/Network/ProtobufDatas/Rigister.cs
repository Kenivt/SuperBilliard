using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSRigister : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.Rigister;

        public override void Clear()
        {
            username_ = string.Empty;
            password_ = string.Empty;
        }
    }
    public partial class SCRigister : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.Rigister;

        public override void Clear()
        {
            result_ = ReturnResult.Failure;
        }
    }
}
