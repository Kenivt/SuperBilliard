using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCProcessingFriendRequestHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCProcessingFriendRequest;

        public override void Handle(object sender, Packet packet)
        {
            SCProcessingFriendRequest scPacket = (SCProcessingFriendRequest)packet;

            //发送对应的确认事件
            GameEntry.Event.Fire(this, S2CFriendHandlerResultEventArgs.Create(S2CFriendMessageType.ProcessingFriendRequest, scPacket.Result, scPacket.RequesterUserName));
        }
    }
}