using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class StartTurnEventArgs : GameEventArgs
    {
        public static int EventId => typeof(StartTurnEventArgs).GetHashCode();

        public override int Id => EventId;

        public bool IsPlaceWhite { get; private set; }

        public override void Clear()
        {

        }

        public static StartTurnEventArgs Create(bool isPlaceWhite)
        {
            StartTurnEventArgs testEvent = ReferencePool.Acquire<StartTurnEventArgs>();
            testEvent.IsPlaceWhite = isPlaceWhite;
            return testEvent;
        }
    }
}
