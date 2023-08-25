using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class CSGetFriendRequestList : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSGetFriendRequestList;

        public override void Clear()
        {
            UserName = "DEFAULT";
        }

        public static CSGetFriendRequestList Create(string userName)
        {
            CSGetFriendRequestList packet = ReferencePool.Acquire<CSGetFriendRequestList>();
            packet.UserName = userName;
            return packet;
        }
    }
}