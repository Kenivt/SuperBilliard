using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCInviteeEnterRoomHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCInviteeEnterRoom;

        public override void Handle(object sender, Packet packet)
        {
            var sCInviteeEnterRoom = (SCInviteeEnterRoom)packet;

            FriendRoomDataBundle dataBundle = GameEntry.DataBundle.GetData<FriendRoomDataBundle>();
            //设置对应的数据
            dataBundle.AddPlayer(sCInviteeEnterRoom.InviteeUsername, false);
        }
    }
}