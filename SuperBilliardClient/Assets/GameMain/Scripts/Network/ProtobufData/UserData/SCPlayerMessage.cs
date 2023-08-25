using SuperBilliard;

namespace GameMessage
{
    public partial class SCPlayerMessage : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCPlayerMessage;

        public override void Clear()
        {
            pmUserName_ = "default";
            pmDescription_ = "default";
            pmIconId_ = 0;
            pmLevel_ = 0;
            pmSnikName_ = "default";
            PmBodyId = 1;
            PmFaceId = 1;
            PmHairId = 1;
            PmKitId = 1;
        }
    }
}