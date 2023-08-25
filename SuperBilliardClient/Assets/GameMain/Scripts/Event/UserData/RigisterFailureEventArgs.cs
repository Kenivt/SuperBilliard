using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class RigisterFailureEventArgs : GameEventArgs
    {
        public static int EventId => typeof(RigisterFailureEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }
        public static RigisterFailureEventArgs Create()
        {
            RigisterFailureEventArgs testEvent = ReferencePool.Acquire<RigisterFailureEventArgs>();
            return testEvent;
        }
    }
}
