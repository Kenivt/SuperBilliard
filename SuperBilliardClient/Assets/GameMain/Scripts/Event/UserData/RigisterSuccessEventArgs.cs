using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class RigisterSuccessEventArgs : GameEventArgs
    {
        public static int EventId => typeof(RigisterSuccessEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }
        public static RigisterSuccessEventArgs Create()
        {
            RigisterSuccessEventArgs testEvent = ReferencePool.Acquire<RigisterSuccessEventArgs>();
            return testEvent;
        }
    }
}
