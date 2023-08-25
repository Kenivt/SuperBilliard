using GameMessage;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SCLeaveFriendRoomHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCLeaveFriendRoom;

        public override void Handle(object sender, Packet packet)
        {
            var sCLeaveFriendRoom = (SCLeaveRoom)packet;

            FriendRoomDataBundle dataBundle = GameEntry.DataBundle.GetData<FriendRoomDataBundle>();
            PlayerDataBundle data = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            Log.Warning("{0} leave room", sCLeaveFriendRoom.Username);
            dataBundle.RemovePlayer(sCLeaveFriendRoom.Username);
        }
    }
}