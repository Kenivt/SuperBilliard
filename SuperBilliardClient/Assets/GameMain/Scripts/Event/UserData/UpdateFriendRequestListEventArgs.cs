using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class UpdateFriendRequestListEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdateFriendRequestListEventArgs).GetHashCode();

        public override int Id => EventId;

        public List<FriendRequestData> FriendRequestList { get; private set; }

        public override void Clear()
        {
            FriendRequestList = null;
        }

        public static UpdateFriendRequestListEventArgs Create(List<FriendRequestData> datas)
        {
            UpdateFriendRequestListEventArgs testEvent = ReferencePool.Acquire<UpdateFriendRequestListEventArgs>();
            testEvent.FriendRequestList = datas;
            return testEvent;
        }
    }
}
