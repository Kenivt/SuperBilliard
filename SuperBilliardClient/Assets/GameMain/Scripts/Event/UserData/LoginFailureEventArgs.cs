using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class LoginFailureEventArgs: GameEventArgs
    {
        public static int EventId => typeof(LoginFailureEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }
        public static LoginFailureEventArgs Create()
        {
            LoginFailureEventArgs testEvent = ReferencePool.Acquire<LoginFailureEventArgs>();
            return testEvent;
        }
    }
}
