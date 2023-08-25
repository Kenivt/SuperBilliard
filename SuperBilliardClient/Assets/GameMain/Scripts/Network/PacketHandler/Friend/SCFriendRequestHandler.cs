using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCFriendRequestHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCRequestFriend;

        public override void Handle(object sender, Packet packet)
        {
            SCRequestFriend scPacket = (SCRequestFriend)packet;

            //发送成功事件
            GameEntry.Event.Fire(this, S2CFriendHandlerResultEventArgs.Create(S2CFriendMessageType.FriendRequest, scPacket.Result, scPacket.TargetUserName));
        }
    }
}