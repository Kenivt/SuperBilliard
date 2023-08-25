using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class LoginSuccessEventArgs : GameEventArgs
    {
        public static int EventId => typeof(LoginSuccessEventArgs).GetHashCode();

        public override int Id => EventId;

        public string userName { get; private set; }
        public override void Clear()
        {
            userName = string.Empty;
        }
        public static LoginSuccessEventArgs Create(string username)
        {
            LoginSuccessEventArgs testEvent = ReferencePool.Acquire<LoginSuccessEventArgs>();
            testEvent.userName = username;
            return testEvent;
        }
    }
}
