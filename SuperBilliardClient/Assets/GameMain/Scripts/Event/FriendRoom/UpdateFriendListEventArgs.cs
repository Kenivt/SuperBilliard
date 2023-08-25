using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class UpdateFriendListEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdateFriendListEventArgs).GetHashCode();

        public override int Id => EventId;

        public List<FriendMessage> FriendList { get; private set; }

        public static UpdateFriendListEventArgs Create(List<FriendMessage> friendMessages)
        {
            UpdateFriendListEventArgs testEvent = ReferencePool.Acquire<UpdateFriendListEventArgs>();
            testEvent.FriendList = friendMessages;
            return testEvent;
        }

        public override void Clear()
        {
            FriendList = null;
        }
    }
}
