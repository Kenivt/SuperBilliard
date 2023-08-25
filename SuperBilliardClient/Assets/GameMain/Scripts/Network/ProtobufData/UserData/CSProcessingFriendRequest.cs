using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class CSProcessingFriendRequest : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSProcessingFriendRequest;

        public override void Clear()
        {
            OwnUsername = "Default";
            RequesterUserName = "Default";
            RequestState = FriendRequestState.Await;
        }

        public static CSProcessingFriendRequest Create(string username, string requesterUserName, FriendRequestState state)
        {
            CSProcessingFriendRequest packet = ReferencePool.Acquire<CSProcessingFriendRequest>();

            packet.OwnUsername = username;
            packet.RequesterUserName = requesterUserName;
            packet.RequestState = state;

            return packet;
        }
    }

    public partial class SCProcessingFriendRequest : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCProcessingFriendRequest;

        public override void Clear()
        {
            result_ = FriendHandleResult.None;
            requesterUserName_ = "Default";
        }
    }
}