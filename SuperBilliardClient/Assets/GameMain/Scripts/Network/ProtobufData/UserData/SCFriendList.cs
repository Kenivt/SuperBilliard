using SuperBilliard;
namespace GameMessage
{
    public partial class SCFriendList : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCFriendListMessage;

        public override void Clear()
        {
            FriendMessages.Clear();
            result_ = FriendHandleResult.None;
        }
    }
}