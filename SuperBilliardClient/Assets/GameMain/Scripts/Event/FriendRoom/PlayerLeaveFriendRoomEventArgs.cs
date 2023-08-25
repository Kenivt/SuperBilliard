using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class PlayerLeaveFriendRoomEventArgs : GameEventArgs
    {
        public static int EventId => typeof(PlayerLeaveFriendRoomEventArgs).GetHashCode();

        public override int Id => EventId;

        public string UserName { get; private set; }

        public override void Clear()
        {
            UserName = "DEFAULT";
        }

        public static PlayerLeaveFriendRoomEventArgs Create(string username)
        {
            PlayerLeaveFriendRoomEventArgs testEvent = ReferencePool.Acquire<PlayerLeaveFriendRoomEventArgs>();
            testEvent.UserName = username;
            return testEvent;
        }
    }
}
