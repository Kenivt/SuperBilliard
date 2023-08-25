using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class StartTimeCountEventArgs : GameEventArgs
    {
        public static int EventId => typeof(StartTimeCountEventArgs).GetHashCode();

        public override int Id => EventId;

        public float Time
        {
            get; private set;
        }

        public override void Clear()
        {
            Time = 40f;
        }

        public static StartTimeCountEventArgs Create(float time)
        {
            StartTimeCountEventArgs testEvent = ReferencePool.Acquire<StartTimeCountEventArgs>();
            testEvent.Time = time;
            return testEvent;
        }
    }
}
