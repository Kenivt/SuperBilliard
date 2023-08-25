using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class StopSyncEventArgs: GameEventArgs
    {
        public static int EventId => typeof(StopSyncEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }
        public static StopSyncEventArgs Create()
        {
            StopSyncEventArgs testEvent = ReferencePool.Acquire<StopSyncEventArgs>();
            return testEvent;
        }
    }
}
