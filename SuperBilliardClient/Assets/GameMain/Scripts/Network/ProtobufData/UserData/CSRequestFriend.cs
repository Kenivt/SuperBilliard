using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class CSRequestFriend : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSRequestFriend;

        public override void Clear()
        {
            OwnUsername = "DEFAULT";
            TargetUsername = "DEFAULT";
        }

        public static CSRequestFriend Create(string ownUsername, string targetUsername)
        {
            CSRequestFriend packet = ReferencePool.Acquire<CSRequestFriend>();
            packet.OwnUsername = ownUsername;
            packet.TargetUsername = targetUsername;
            return packet;
        }
    }
    public partial class SCRequestFriend : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCRequestFriend;

        public override void Clear()
        {
            result_ = FriendHandleResult.None;
            targetUserName_ = "DEFAULT";
        }
    }
}