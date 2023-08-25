using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class TimerOutEventArgs : GameEventArgs
    {
        public static int EventId => typeof(TimerOutEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }

        public static TimerOutEventArgs Create()
        {
            TimerOutEventArgs testEvent = ReferencePool.Acquire<TimerOutEventArgs>();
            return testEvent;
        }
    }
}
