using GameMessage;
using UnityEngine;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SCBilliardSyncHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCBilliardSync;
        private float _lastTime;
        private float _syncCount = 0;
        public override void Handle(object sender, Packet packet)
        {
            SCBilliardSync sCBallClubSync = (SCBilliardSync)packet;
            BilliardMessage[] billiardMessages = new BilliardMessage[sCBallClubSync.BallMessages.Count];
            int index = 0;
            foreach (var item in sCBallClubSync.BallMessages)
            {
                billiardMessages[index].BilliardId = item.BallId;
                billiardMessages[index].Position = new UnityEngine.Vector3(item.Position.X, item.Position.Y, item.Position.Z);
                billiardMessages[index].Rotation = new UnityEngine.Vector3(item.Rotation.X, item.Rotation.Y, item.Rotation.Z);
                index++;
            }
            _lastTime = Time.time;
            GameEntry.Event.FireNow(this, SyncBilliardEventArgs.Create(billiardMessages));
        }
    }
}
