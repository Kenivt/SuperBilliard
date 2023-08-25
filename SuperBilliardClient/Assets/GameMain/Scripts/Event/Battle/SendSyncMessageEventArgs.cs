using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class SendSyncMessageEventArgs : GameEventArgs
    {
        public static int EventId => typeof(SendSyncMessageEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }
        public static SendSyncMessageEventArgs Create()
        {
            SendSyncMessageEventArgs testEvent = ReferencePool.Acquire<SendSyncMessageEventArgs>();
            return testEvent;
        }
    }
}
