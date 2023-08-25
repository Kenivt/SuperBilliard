using GameMessage;
using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public enum S2CFriendMessageType
    {
        None,
        FriendRequest,
        ProcessingFriendRequest,
        FriendList,
    }

    public class S2CFriendHandlerResultEventArgs : GameEventArgs
    {
        public static int EventId => typeof(S2CFriendHandlerResultEventArgs).GetHashCode();

        public override int Id => EventId;

        public S2CFriendMessageType MesssageType { get; private set; }

        public FriendHandleResult Result { get; private set; }

        public object UserData { get; private set; }

        public override void Clear()
        {
            MesssageType = S2CFriendMessageType.None;
        }
        public static S2CFriendHandlerResultEventArgs Create(S2CFriendMessageType type, FriendHandleResult returnResult, object userdata)
        {
            S2CFriendHandlerResultEventArgs testEvent = ReferencePool.Acquire<S2CFriendHandlerResultEventArgs>();
            testEvent.MesssageType = type;
            testEvent.Result = returnResult;
            testEvent.UserData = userdata;
            return testEvent;
        }
    }
}
