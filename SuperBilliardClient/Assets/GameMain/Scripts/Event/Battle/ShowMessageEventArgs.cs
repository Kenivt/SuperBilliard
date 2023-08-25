using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class ShowMessageEventArgs : GameEventArgs
    {
        public static int EventId => typeof(ShowMessageEventArgs).GetHashCode();

        public override int Id => EventId;

        public string Message
        {
            get; private set;
        }
        public override void Clear()
        {
            Message = string.Empty;
        }
        public static ShowMessageEventArgs Create(string message)
        {
            ShowMessageEventArgs testEvent = ReferencePool.Acquire<ShowMessageEventArgs>();
            testEvent.Message = message;
            return testEvent;
        }
    }
}
