using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class RecieveFriendListEventArgs : GameEventArgs
    {
        public static int EventId => typeof(RecieveFriendListEventArgs).GetHashCode();

        public override int Id => EventId;

        public List<FriendMessage> FriendMessages { get; private set; }

        public override void Clear()
        {

        }

        public static RecieveFriendListEventArgs Create(List<FriendMessage> FriendMessage)
        {
            RecieveFriendListEventArgs testEvent = ReferencePool.Acquire<RecieveFriendListEventArgs>();
            testEvent.FriendMessages = FriendMessage;
            return testEvent;
        }
    }
}
