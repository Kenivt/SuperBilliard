using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class BackMainMenuEventArgs : GameEventArgs
    {
        public static int EventId => typeof(BackMainMenuEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }

        public static BackMainMenuEventArgs Create()
        {
            BackMainMenuEventArgs testEvent = ReferencePool.Acquire<BackMainMenuEventArgs>();
            return testEvent;
        }
    }
}
