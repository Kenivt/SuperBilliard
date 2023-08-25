using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class UpdateFriendStateEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdateFriendStateEventArgs).GetHashCode();

        public override int Id => EventId;

        public string UserName { get; private set; }

        public GameMessage.PlayerStatus Status { get; private set; }

        public override void Clear()
        {
            UserName = string.Empty;
            Status = GameMessage.PlayerStatus.PlayerStausNone;
        }

        public static UpdateFriendStateEventArgs Create(string username, GameMessage.PlayerStatus status)
        {
            UpdateFriendStateEventArgs testEvent = ReferencePool.Acquire<UpdateFriendStateEventArgs>();
            testEvent.UserName = username;
            testEvent.Status = status;
            return testEvent;
        }
    }
}