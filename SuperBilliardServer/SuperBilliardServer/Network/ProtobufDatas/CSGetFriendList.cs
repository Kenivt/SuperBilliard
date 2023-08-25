using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSGetFriendList : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSGetFriendMessage;

        public override void Clear()
        {
            username_ = "Default";
        }
    }

    public partial class PlayerMessage : IReference
    {
        public void Clear()
        {
            username_ = "Default";
            nickName_ = "Default";
            description_ = "Default";
            HairId = 1;
            FaceId = 1;
            KitId = 1;
            BodyId = 1;
            IsLogin = false;
        }

        public static PlayerMessage Create()
        {
            PlayerMessage playerMessage = ReferencePool.Acquire<PlayerMessage>();
            return playerMessage;
        }
    }

    public partial class SCFriendList : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCFriendListMessage;

        public override void Clear()
        {
            foreach (var item in friendMessages_)
            {
                ReferencePool.Release(item);
            }
            FriendMessages.Clear();
            result_ = FriendHandleResult.None;
        }
        public static SCFriendList Create()
        {
            SCFriendList scFriendListMessage = ReferencePool.Acquire<SCFriendList>();
            return scFriendListMessage;
        }
    }

    public partial class CSRequestFriend : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSRequestFriend;

        public override void Clear()
        {
            ownUsername_ = "Default";
            targetUsername_ = "Default";
        }
    }

    public partial class SCRequestFriend : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCRequestFriend;

        public static SCRequestFriend Create()
        {
            SCRequestFriend sCRequestFriend = ReferencePool.Acquire<SCRequestFriend>();
            return sCRequestFriend;
        }

        public override void Clear()
        {
            targetUserName_ = "Default";
            result_ = FriendHandleResult.None;
        }
    }
}