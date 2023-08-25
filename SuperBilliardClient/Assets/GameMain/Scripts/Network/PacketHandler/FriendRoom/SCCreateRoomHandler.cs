using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCCreateRoomHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCCreateGame;

        public override void Handle(object sender, Packet packet)
        {
            var sCCreateRoom = (SCCreateRoom)packet;

            FriendRoomDataBundle dataBundle = GameEntry.DataBundle.GetData<FriendRoomDataBundle>();

            //设置对应的数据
            dataBundle.RoomId = sCCreateRoom.RoomId;
            dataBundle.GameType = sCCreateRoom.GameType;

            //把自己加入到房间中
            string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            dataBundle.AddPlayer(username, false);
            //打开好友房间
            GameEntry.UI.OpenUIForm(EnumUIForm.FriendRoom);
        }
    }
}