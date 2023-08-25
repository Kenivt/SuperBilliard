using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class UpdatePlayerReadyStateEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdatePlayerReadyStateEventArgs).GetHashCode();

        public override int Id => EventId;

        public string UserName { get; private set; }

        public bool IsReady { get; private set; }
        public override void Clear()
        {
            UserName = default;
            IsReady = false;
        }
        public static UpdatePlayerReadyStateEventArgs Create(string username, bool isReady)
        {
            UpdatePlayerReadyStateEventArgs testEvent = ReferencePool.Acquire<UpdatePlayerReadyStateEventArgs>();

            testEvent.UserName = username;
            testEvent.IsReady = isReady;

            return testEvent;
        }
    }
}
