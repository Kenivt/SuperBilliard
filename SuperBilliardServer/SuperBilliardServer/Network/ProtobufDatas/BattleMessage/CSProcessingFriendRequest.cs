using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSProcessingFriendRequest : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSProcessingFriendRequest;

        public override void Clear()
        {
            ownUsername_ = "default";
            requesterUserName_ = "default";
            requestState_ = 0;
        }
    }

    public partial class SCProcessingFriendRequest : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCProcessingFriendRequest;

        public static SCProcessingFriendRequest Create()
        {
            SCProcessingFriendRequest sCProcessingFriendRequest = ReferencePool.Acquire<SCProcessingFriendRequest>();
            return sCProcessingFriendRequest;
        }

        public override void Clear()
        {
            requesterUserName_ = "default";
            result_ = FriendHandleResult.None;
        }
    }
}
