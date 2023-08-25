using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCAcceptGameInvitationHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCAcceptGameInvitation;

        public override void Handle(object sender, Packet packet)
        {
            var sc = (SCAcceptGameInvitation)packet;
            //接收了对方的邀请
            //设置对应的数据
            FriendRoomDataBundle dataBundle = GameEntry.DataBundle.GetData<FriendRoomDataBundle>();
            //加入房间
            dataBundle.AddPlayer(sc.InviterUsername, sc.IsReady);
            dataBundle.AddPlayer(sc.OwnUsername, false);
            //打开对应的房间界面
            GameEntry.UI.OpenUIForm(EnumUIForm.FriendRoom);
        }
    }
}