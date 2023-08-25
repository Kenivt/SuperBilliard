using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class InviteeEnterRoomEventArgs : GameEventArgs
    {
        public static int EventId => typeof(InviteeEnterRoomEventArgs).GetHashCode();

        public override int Id => EventId;

        public PlayerData playerData { get; private set; }

        public override void Clear()
        {
            playerData = null;
        }

        public static InviteeEnterRoomEventArgs Create(PlayerData playerData)
        {
            InviteeEnterRoomEventArgs testEvent = ReferencePool.Acquire<InviteeEnterRoomEventArgs>();
            testEvent.playerData = playerData;
            return testEvent;
        }
    }
}
