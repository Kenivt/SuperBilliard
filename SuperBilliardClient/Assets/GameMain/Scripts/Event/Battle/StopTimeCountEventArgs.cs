using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class StopTimeCountEventArgs : GameEventArgs
    {
        public static int EventId => typeof(StopTimeCountEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }

        public static StopTimeCountEventArgs Create()
        {
            StopTimeCountEventArgs testEvent = ReferencePool.Acquire<StopTimeCountEventArgs>();
            return testEvent;
        }
    }
}
