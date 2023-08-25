using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class AllBilliardStopEventArgs : GameEventArgs
    {
        public static int EventId => typeof(AllBilliardStopEventArgs).GetHashCode();

        public override int Id => EventId;

        public int FristCollideId { get; private set; }

        public override void Clear()
        {
            FristCollideId = -1;
        }

        public static AllBilliardStopEventArgs Create(int fristCollideId)
        {
            AllBilliardStopEventArgs testEvent = ReferencePool.Acquire<AllBilliardStopEventArgs>();
            testEvent.FristCollideId = fristCollideId;
            return testEvent;
        }
    }
}
