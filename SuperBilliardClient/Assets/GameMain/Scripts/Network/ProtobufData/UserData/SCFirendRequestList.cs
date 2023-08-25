using SuperBilliard;

namespace GameMessage
{
    public partial class SCFriendRequestList : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCFriendRequestList;

        public override void Clear()
        {
            requestFriendList_.Clear();
            result_ = FriendHandleResult.None;
        }
    }
}