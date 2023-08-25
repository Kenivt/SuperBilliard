using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSGetFriendRequestList : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSGetFriendRequestList;
        public override void Clear()
        {
            userName_ = "DEFAULT";
        }
    }

    public partial class SCFriendRequestList : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCFriendRequestList;
        public override void Clear()
        {
            foreach (var item in requestFriendList_)
            {
                ReferencePool.Release(item);
            }
            requestFriendList_.Clear();
        }
        public static SCFriendRequestList Create()
        {
            SCFriendRequestList scFriendRequestList = ReferencePool.Acquire<SCFriendRequestList>();
            return scFriendRequestList;
        }
    }
}
