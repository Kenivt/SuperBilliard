using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class CSGetFriendList : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSGetFriendMessage;

        public static CSGetFriendList Create(string username)
        {
            CSGetFriendList cSGetFriendMessage = ReferencePool.Acquire<CSGetFriendList>();
            cSGetFriendMessage.Username = username;
            return cSGetFriendMessage;
        }
        public override void Clear()
        {
            Username = "default";
        }
    }
}