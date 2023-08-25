using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSTurnAnalysis : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSTurnAnalysis;
        public override void Clear()
        {
            fristCollideBIlliardId_ = -1;
        }
        public static CSTurnAnalysis Create(int firstId)
        {
            CSTurnAnalysis pack = ReferencePool.Acquire<CSTurnAnalysis>();
            pack.fristCollideBIlliardId_ = firstId;
            return pack;
        }

    }

    public partial class SCTurnAnalysis : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCTurnAnalysis;
        public override void Clear()
        {
            fristCollideBIlliardId_ = -1;
        }

        public static SCTurnAnalysis Create(int firstId)
        {
            SCTurnAnalysis pack = ReferencePool.Acquire<SCTurnAnalysis>();
            pack.fristCollideBIlliardId_ = firstId;
            return pack;
        }

    }
}
