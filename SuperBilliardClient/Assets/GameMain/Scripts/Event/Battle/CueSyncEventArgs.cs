using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class CueSyncEventArgs : GameEventArgs
    {
        public static int EventId => typeof(CueSyncEventArgs).GetHashCode();

        public override int Id => EventId;

        public float angleY;
        public Vector3 position;

        public override void Clear()
        {
            angleY = 0;
            position = new Vector3(500, 500, 500);
        }
        public static CueSyncEventArgs Create(Vector3 position, float angleY)
        {
            CueSyncEventArgs testEvent = ReferencePool.Acquire<CueSyncEventArgs>();
            testEvent.position = position;
            testEvent.angleY = angleY;
            return testEvent;
        }
    }
}
