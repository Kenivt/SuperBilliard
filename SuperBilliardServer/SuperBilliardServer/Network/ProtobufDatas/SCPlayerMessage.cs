using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCPlayerMessage : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCPlayerMessage;
        public override void Clear()
        {
            pmDescription_ = "default";
            pmIconId_ = -1;
            PmSnikName = "default";
            PmLevel = -1;
        }
        public SCPlayerMessage Create(string username, string snikName, int iconId, int level, string description)
        {
            SCPlayerMessage sCPlayerMessage = ReferencePool.Acquire<SCPlayerMessage>();
            sCPlayerMessage.PmUserName = username;
            sCPlayerMessage.pmDescription_ = description;
            sCPlayerMessage.pmIconId_ = iconId;
            sCPlayerMessage.PmSnikName = snikName;
            sCPlayerMessage.PmLevel = level;
            return sCPlayerMessage;
        }
    }
}
