using GameFramework;
using SuperBilliardServer.Network.Packets;

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
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSCueSync;
        public override void Clear()
        {
            position_ = null;
            angltY_ = 0;
        }
    }

    public partial class SCCueSync : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCCueSync;
        public override void Clear()
        {
            ReferencePool.Release(position_);
            position_ = null;
        }
        public static SCCueSync Create(Vector3 position, float angleY)
        {
            SCCueSync clubMessage = ReferencePool.Acquire<SCCueSync>();
            clubMessage.position_ = Vector3Mess.Create(position);
            clubMessage.AngltY = angleY;
            return clubMessage;
        }
    }

    public partial class SCCueStorageSync : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCCueStorageSync;
        public override void Clear()
        {
            FillAmount = 0;
            dirX_ = 0;
            dirY_ = 0;
            dirZ_ = 0;
        }
        public static SCCueStorageSync Create(float fillAmount, Vector3 vector3)
        {
            SCCueStorageSync cuemessage = ReferencePool.Acquire<SCCueStorageSync>();
            cuemessage.FillAmount = fillAmount;
            cuemessage.DirX = vector3.x;
            cuemessage.DirY = vector3.y;
            cuemessage.DirZ = vector3.z;
            return cuemessage;
        }
    }

    public partial class CSCueStorageSync : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSCueStorageSync;
        public override void Clear()
        {
            FillAmount = 0;

        }
    }

    public partial class CSBilliardSync : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSBilliardSync;

        public override void Clear()
        {
            ballMessages_.Clear();
        }
    }

    public partial class SCBilliardSync : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCBilliardSync;

        public override void Clear()
        {
            foreach (var item in ballMessages_)
            {
                ReferencePool.Release(item);
            }
            ballMessages_.Clear();
        }
        public static SCBilliardSync Create(List<BallMessage> ballMessages)
        {
            SCBilliardSync ballClubSync = ReferencePool.Acquire<SCBilliardSync>();
            foreach (var item in ballMessages)
            {
                ballClubSync.BallMessages.Add(BallMessage.Create(item.BallId,
                                       new Vector3(item.Position.X, item.Position.Y, item.Position.Z)
                                                          , new Vector3(item.Rotation.X, item.Rotation.Y, item.Rotation.Z)));
            }
            return ballClubSync;
        }
    }

    public partial class SCBilliardTypeConfirm : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCConfirmBilliardType;

        public override void Clear()
        {
            ScBilliardType = BilliardType.None;
        }
        public static SCBilliardTypeConfirm Create(BilliardType billiardType)
        {
            SCBilliardTypeConfirm billiardTypeConfirm = ReferencePool.Acquire<SCBilliardTypeConfirm>();
            billiardTypeConfirm.ScBilliardType = billiardType;
            return billiardTypeConfirm;
        }
    }
    public partial class CSBilliardTypeConfirm : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSConfirmBilliardType;

        public override void Clear()
        {
            CsBilliardType = BilliardType.None;
        }
    }

    public partial class CSGameResultPack : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.GameResult;
        public override void Clear()
        {
            result_ = GameResult.None;
        }
    }

    public partial class SCGameResultPack : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.GameResult;

        public override void Clear()
        {
            result_ = GameResult.None;
        }
        public static SCGameResultPack Create(GameResult gameResult)
        {
            SCGameResultPack pack = ReferencePool.Acquire<SCGameResultPack>();
            pack.result_ = gameResult;
            return pack;
        }
    }

    public partial class SCGameStart : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCGameStart;

        public override void Clear()
        {

        }

        public static SCGameStart Create()
        {
            SCGameStart packet = ReferencePool.Acquire<SCGameStart>();
            return packet;
        }
    }
}