using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class CueChangeEventArgs : GameEventArgs
    {
        public static int EventId => typeof(CueChangeEventArgs).GetHashCode();

        public override int Id => EventId;
        public float EulerY { get; private set; }
        public Vector3 Position { get; private set; }

        public override void Clear()
        {
            Position = Vector3.zero;
            EulerY = 0;
        }

        public static CueChangeEventArgs Create(Vector3 position, float eulerY)
        {
            CueChangeEventArgs testEvent = ReferencePool.Acquire<CueChangeEventArgs>();
            testEvent.Position = position;
            testEvent.EulerY = eulerY;
            return testEvent;
        }
    }
}
