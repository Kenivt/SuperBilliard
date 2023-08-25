using GameFramework;
using SuperBilliardServer.Constant;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{

    public partial class CSJoinGame : CSPacketBase
    {
        public override int Id => PacketTypeId.CSJoinGame;

        public override void Clear()
        {
            UserName = "default";
            GameType = GameType.FancyMatch;
        }
        public static CSJoinGame Create(string username, GameType gameType)
        {
            CSJoinGame packet = ReferencePool.Acquire<CSJoinGame>();
            packet.UserName = username;
            packet.GameType = gameType;
            return packet;
        }
    }

    public partial class CSStopMatch : CSPacketBase
    {
        public override int Id => PacketTypeId.CSStopMatch;

        public override void Clear()
        {
            userName_ = "Default";
        }
        public static CSStopMatch Create()
        {
            CSStopMatch packet = ReferencePool.Acquire<CSStopMatch>();
            packet.userName_ = "Default";
            return packet;
        }
    }

    public partial class SCStartLoadGame : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCStartLoadGame;
        public override void Clear()
        {
            gameType_ = GameType.None;
            isFirstMove_ = false;
            opponentName_ = "Default";
            randomSeed_ = 114514;
        }

        public static SCStartLoadGame Create()
        {
            SCStartLoadGame packet = ReferencePool.Acquire<SCStartLoadGame>();
            return packet;
        }
    }

    public partial class CSLoadGameComplete : CSPacketBase
    {
        public override int Id => PacketTypeId.CSLoadGameComplete;
        public override void Clear()
        {

        }

        public static CSLoadGameComplete Create()
        {
            CSLoadGameComplete packet = ReferencePool.Acquire<CSLoadGameComplete>();
            return packet;
        }
    }

    public partial class CSOpenKcp : CSPacketBase
    {
        public override int Id => PacketTypeId.CSOpenKcp;
        public override void Clear()
        {
            kcpIpEnd_ = "NULL";
        }

        public static CSOpenKcp Create(string kcpIpEnd)
        {
            CSOpenKcp packet = ReferencePool.Acquire<CSOpenKcp>();
            packet.kcpIpEnd_ = kcpIpEnd;
            return packet;
        }
    }
    public partial class SCOpenKcp : SCPacketBase
    {
        public override int Id => PacketTypeId.SCOpenKcp;

        public override void Clear()
        {
            kcpIpEnd_ = "NULL";
        }
        public static SCOpenKcp Create(string kcpIpEnd)
        {
            SCOpenKcp packet = ReferencePool.Acquire<SCOpenKcp>();
            packet.kcpIpEnd_ = kcpIpEnd;
            return packet;
        }
    }
}