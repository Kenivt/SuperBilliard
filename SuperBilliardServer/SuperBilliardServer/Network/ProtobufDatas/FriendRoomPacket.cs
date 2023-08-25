using GameFramework;
using SuperBilliardServer.Constant;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSCreateRoom : CSPacketBase
    {
        public override int Id => PacketTypeId.CSCreateGame;
        public override void Clear()
        {
            gameType_ = GameType.None;
            username_ = "DEFAULT";
        }
        public static CSCreateRoom Create()
        {
            CSCreateRoom packet = ReferencePool.Acquire<CSCreateRoom>();
            return packet;
        }
    }

    public partial class SCCreateRoom : SCPacketBase
    {
        public override int Id => PacketTypeId.SCCreateGame;
        public override void Clear()
        {
            gameType_ = GameType.None;
            roomId_ = 0;
        }
        public static SCCreateRoom Create()
        {
            SCCreateRoom packet = ReferencePool.Acquire<SCCreateRoom>();
            return packet;
        }
    }

    public partial class CSInviteFriendBattle : CSPacketBase
    {
        public override int Id => PacketTypeId.CSInviteFriendBattle;

        public override void Clear()
        {
            gameType_ = GameType.None;
            inviteeUserName_ = "DEFAULT";
            ownUserName_ = "DEFAULT";
            roomId_ = 0;
        }
        public static CSInviteFriendBattle Create(GameType gameType, string ownUserName, string inviteeUserName, int roomId)
        {
            CSInviteFriendBattle packet = ReferencePool.Acquire<CSInviteFriendBattle>();
            packet.gameType_ = gameType;
            packet.ownUserName_ = ownUserName;
            packet.inviteeUserName_ = inviteeUserName;
            packet.roomId_ = roomId;
            return packet;
        }
    }

    public partial class SCInviteFriendBattle : SCPacketBase
    {
        public override int Id => PacketTypeId.SCInviteFriendBattle;

        public override void Clear()
        {
            gameType_ = GameType.None;
            inviteeUsername_ = "DEFAULT";
            roomId_ = 0;
        }
        public static SCInviteFriendBattle Create(GameType gametype, string inviteeUsername, int roomId)
        {
            SCInviteFriendBattle packet = ReferencePool.Acquire<SCInviteFriendBattle>();
            packet.gameType_ = gametype;
            packet.inviteeUsername_ = inviteeUsername;
            packet.roomId_ = roomId;
            return packet;
        }
    }

    public partial class CSAcceptGameInvitation : CSPacketBase
    {
        public override int Id => PacketTypeId.CSAcceptGameInvitation;

        public override void Clear()
        {
            gameType_ = GameType.None;
            inviterUserName_ = "DEFAULT";
            ownUsername_ = "DEFAULT";
            roomId_ = 0;
        }
        public static CSAcceptGameInvitation Create(GameType gameType, string inviterUserName, string ownUserName, int roomId)
        {
            CSAcceptGameInvitation packet = ReferencePool.Acquire<CSAcceptGameInvitation>();
            packet.gameType_ = gameType;
            packet.inviterUserName_ = inviterUserName;
            packet.ownUsername_ = ownUserName;
            packet.roomId_ = roomId;
            return packet;
        }
    }

    public partial class SCAcceptGameInvitation : SCPacketBase
    {
        public override int Id => PacketTypeId.SCAcceptGameInvitation;

        public override void Clear()
        {
            gameType_ = GameType.None;
            inviterUsername_ = "DEFAULT";
            ownUsername_ = "DEFAULT";
            result_ = InvitationResult.None;
            isReady_ = false;
        }
        public static SCAcceptGameInvitation Create(GameType gameType, string inviterUsername,
            bool isReady, string ownUserName, InvitationResult result)
        {
            SCAcceptGameInvitation packet = ReferencePool.Acquire<SCAcceptGameInvitation>();
            packet.gameType_ = gameType;
            packet.inviterUsername_ = inviterUsername;
            packet.ownUsername_ = ownUserName;
            packet.result_ = result;
            packet.isReady_ = isReady;
            return packet;
        }
    }

    /// <summary>
    /// 邀请者进入房间
    /// </summary>
    public partial class SCInviteeEnterRoom : SCPacketBase
    {
        public override int Id => PacketTypeId.SCInviteeEnterRoom;

        public override void Clear()
        {
            gameType_ = GameType.None;
            inviteeUsername_ = "DEFAULT";
        }
        public static SCInviteeEnterRoom Create(GameType gameType, string inviteeUserName)
        {
            SCInviteeEnterRoom packet = ReferencePool.Acquire<SCInviteeEnterRoom>();
            packet.gameType_ = gameType;
            packet.inviteeUsername_ = inviteeUserName;
            return packet;
        }
    }

    public partial class CSLeaveRoom : CSPacketBase
    {
        public override int Id => PacketTypeId.CSLeaveFriendRoom;

        public override void Clear()
        {
            username_ = "DEFAULT";
        }

        public static CSLeaveRoom Create(string username)
        {
            CSLeaveRoom packet = ReferencePool.Acquire<CSLeaveRoom>();
            packet.username_ = username;
            return packet;
        }
    }

    public partial class SCLeaveRoom : SCPacketBase
    {
        public override int Id => PacketTypeId.SCLeaveFriendRoom;

        public override void Clear()
        {
            username_ = "DEFAULT";
        }
        public static SCLeaveRoom Create(string username)
        {
            SCLeaveRoom packet = ReferencePool.Acquire<SCLeaveRoom>();
            packet.username_ = username;
            return packet;
        }

    }

    public partial class CSReadyGame : CSPacketBase
    {
        public override int Id => PacketTypeId.CSReadyGame;
        public override void Clear()
        {
            username_ = "DEFAULT";
            isReady_ = false;
        }
    }

    public partial class SCReadyGame : SCPacketBase
    {
        public override int Id => PacketTypeId.SCReadyGame;

        public override void Clear()
        {
            username_ = "DEFAULT";
            isReady_ = false;
        }

        public static SCReadyGame Create(string username, bool isReady)
        {
            SCReadyGame packet = ReferencePool.Acquire<SCReadyGame>();
            packet.username_ = username;
            packet.isReady_ = isReady;
            return packet;
        }
    }
}