using UnityEngine;
using GameFramework;
using SuperBilliard;

namespace GameMessage
{
    public partial class BallMessage : IReference
    {
        public void Clear()
        {
            ballId_ = -1;
            ReferencePool.Release(position_);
            ReferencePool.Release(rotation_);
            position_ = null;
            rotation_ = null;
        }
        public static BallMessage Create(int ballId, Vector3 position, Vector3 rototaion)
        {
            BallMessage ballMessage = ReferencePool.Acquire<BallMessage>();
            ballMessage.ballId_ = ballId;
            ballMessage.position_ = Vector3Mess.Create(position);
            ballMessage.rotation_ = Vector3Mess.Create(rototaion);
            return ballMessage;
        }
    }

    public partial class CSCueSync : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSCueSync;
        public override void Clear()
        {
            ReferencePool.Release(position_);
            position_ = null;
            AngltY = 0;
        }
        public static CSCueSync Create(Vector3 position, float angleY)
        {
            CSCueSync clubMessage = ReferencePool.Acquire<CSCueSync>();
            clubMessage.position_ = Vector3Mess.Create(position);
            clubMessage.AngltY = angleY;
            return clubMessage;
        }
    }

    public partial class SCCueSync : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCCueSync;
        public override void Clear()
        {
            position_ = null;
            AngltY = 0;
        }
    }

    public partial class SCCueStorageSync : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCCueStorageSync;
        public override void Clear()
        {
            FillAmount = 0;
            DirX = 0;
            DirY = 0;
            DirZ = 0;
        }
    }

    public partial class CSCueStorageSync : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSCueStorageSync;
        public override void Clear()
        {
            FillAmount = 0;
            DirX = 0;
            DirY = 0;
            DirZ = 0;
        }
        public static CSCueStorageSync Create(float fillAmount, Vector3 dir)
        {
            CSCueStorageSync cueStorageSync = ReferencePool.Acquire<CSCueStorageSync>();
            cueStorageSync.FillAmount = fillAmount;
            cueStorageSync.DirX = dir.x;
            cueStorageSync.DirY = dir.y;
            cueStorageSync.DirZ = dir.z;
            return cueStorageSync;
        }
    }

    public partial class CSBilliardSync : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSBilliardSync;

        public override void Clear()
        {
            foreach (var item in ballMessages_)
            {
                ReferencePool.Release(item);
            }
            ballMessages_.Clear();
        }
        public static CSBilliardSync Create(SuperBilliard.BilliardMessage[] billiardMessage)
        {
            CSBilliardSync billiardSync = ReferencePool.Acquire<CSBilliardSync>();
            foreach (var item in billiardMessage)
            {
                BallMessage ballMessage = BallMessage.Create(item.BilliardId, item.Position, item.Rotation);
                billiardSync.BallMessages.Add(ballMessage);
            }
            return billiardSync;
        }
    }

    public partial class SCBilliardSync : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCBilliardSync;

        public override void Clear()
        {
            ballMessages_.Clear();
        }
    }

    public partial class CSBilliardTypeConfirm : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSConfirmBilliardType;

        public override void Clear()
        {
            csBilliardType_ = BilliardType.None;
        }
        public static CSBilliardTypeConfirm Create(BilliardType billiardType)
        {
            CSBilliardTypeConfirm billiardTypeConfirm = ReferencePool.Acquire<CSBilliardTypeConfirm>();
            billiardTypeConfirm.CsBilliardType = billiardType;
            return billiardTypeConfirm;
        }
    }

    public partial class SCBilliardTypeConfirm : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCConfirmBilliardType;

        public override void Clear()
        {
            scBilliardType_ = BilliardType.None;
        }
    }

    public partial class CSEndTurn : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSEndTurn;

        public override void Clear()
        {
            isfoul_ = false;
        }

        public static CSEndTurn Create(bool isfoul)
        {
            var packet = ReferencePool.Acquire<CSEndTurn>();
            packet.isfoul_ = isfoul;
            return packet;
        }
    }

    public partial class CSExitGameRoom : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSExitGameRoom;

        public override void Clear()
        {
        }
        public static CSExitGameRoom Create()
        {
            CSExitGameRoom packet = ReferencePool.Acquire<CSExitGameRoom>();
            return packet;
        }
    }

    public partial class CSGameResultPack : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.GameResult;
        public override void Clear()
        {
            result_ = GameResult.None;
        }

        public static CSGameResultPack Create(GameResult gameResult)
        {
            CSGameResultPack pack = ReferencePool.Acquire<CSGameResultPack>();
            pack.result_ = gameResult;
            return pack;
        }
    }

    public partial class SCGameResultPack : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.GameResult;

        public override void Clear()
        {
            result_ = GameResult.None;
        }
    }

    public partial class SCGameStart : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCGameStart;

        public override void Clear()
        {

        }
    }

    public partial class SCStartTurn : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCStartTurn;

        public override void Clear()
        {
            isPlacewhite_ = false;
        }
    }

    public partial class CSSetBilliardState : CSPacketBase
    {

        public override int Id => SuperBilliard.Constant.PacketTypeId.CSSetBilliardState;

        public override void Clear()
        {
            BilliardId = -1;
            Active = false;
            PhysicsIsOpen = false;
        }

        public static CSSetBilliardState Create(int billiardId, bool active, bool physicsIsOpen)
        {
            CSSetBilliardState pack = ReferencePool.Acquire<CSSetBilliardState>();
            pack.BilliardId = billiardId;
            pack.Active = active;
            pack.physicsIsOpen_ = physicsIsOpen;
            return pack;
        }
    }
    public partial class CSTurnAnalysis : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSTurnAnalysis;
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
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCTurnAnalysis;
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

    public partial class SCSetBilliardState : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCSetBilliardState;

        public override void Clear()
        {
            BilliardId = -1;
            Active = false;
            physicsIsOpen_ = false;
        }
        public static SCSetBilliardState Create(int billiardId, bool active, bool physicsIsOpen)
        {
            SCSetBilliardState pack = ReferencePool.Acquire<SCSetBilliardState>();
            pack.BilliardId = billiardId;
            pack.Active = active;
            pack.physicsIsOpen_ = physicsIsOpen;
            return pack;
        }
    }

    public partial class CSSyncSound : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSSyncSound;

        public override void Clear()
        {
            SoundId = 0;
            Volumn = 1;
            ReferencePool.Release(Position);
            Position = null;
        }

        public static CSSyncSound Create(int soundId, float volumn, Vector3 pos)
        {
            CSSyncSound pack = ReferencePool.Acquire<CSSyncSound>();
            pack.SoundId = soundId;
            pack.Volumn = volumn;
            pack.Position = Vector3Mess.Create(pos);
            return pack;
        }
    }

    public partial class SCSyncSound : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCSyncSound;

        public override void Clear()
        {
            SoundId = 0;
            Volumn = 1;
            Position = null;
        }
    }

    public partial class CSBattleEmptyEvent : CSPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.CSBattleEmptyEvent;

        public override void Clear()
        {
            BattleEvent = BattleEmptyEvent.BattleNone;
        }
        public static CSBattleEmptyEvent Create(BattleEmptyEvent battleEvent)
        {
            CSBattleEmptyEvent packet = ReferencePool.Acquire<CSBattleEmptyEvent>();
            packet.BattleEvent = battleEvent;
            return packet;
        }
    }

    public partial class SCBattleEmptyEvent : SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCBattleEmptyEvent;

        public override void Clear()
        {
            BattleEvent = BattleEmptyEvent.BattleNone;
        }
    }
}