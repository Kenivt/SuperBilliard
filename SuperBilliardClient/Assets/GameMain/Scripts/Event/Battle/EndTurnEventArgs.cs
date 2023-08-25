using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class EndTurnEventArgs : GameEventArgs
    {
        public static int EventId => typeof(EndTurnEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }

        public static EndTurnEventArgs Create()
        {
            EndTurnEventArgs testEvent = ReferencePool.Acquire<EndTurnEventArgs>();
            return testEvent;
        }
    }
}
