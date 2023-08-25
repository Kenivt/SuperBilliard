using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace SuperBilliard
{
    public struct BilliardMessage
    {
        public bool needSync;
        public int BilliardId;
        public Vector3 Position;
        public Vector3 Rotation;
    }

    public class SyncBilliardEventArgs : GameEventArgs
    {
        public static int EventId => typeof(SyncBilliardEventArgs).GetHashCode();

        public override int Id => EventId;
        public BilliardMessage[] BilliardMessageList { get; private set; }
        public override void Clear()
        {
            BilliardMessageList = null;
        }

        public static SyncBilliardEventArgs Create(params BilliardMessage[] billiardMessages)
        {
            SyncBilliardEventArgs testEvent = ReferencePool.Acquire<SyncBilliardEventArgs>();
            testEvent.BilliardMessageList = billiardMessages;
            return testEvent;
        }
    }
}
