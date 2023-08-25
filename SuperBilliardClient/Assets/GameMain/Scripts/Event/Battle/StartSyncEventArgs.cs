using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class StartSyncEventArgs: GameEventArgs
    {
        public static int EventId => typeof(StartSyncEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }
        public static StartSyncEventArgs Create()
        {
            StartSyncEventArgs testEvent = ReferencePool.Acquire<StartSyncEventArgs>();
            return testEvent;
        }
    }
}
