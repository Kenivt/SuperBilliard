using GameMessage;
using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class RecieveBattleInvitationEventArgs : GameEventArgs
    {
        public static int EventId => typeof(RecieveBattleInvitationEventArgs).GetHashCode();

        public override int Id => EventId;

        public GameType GameType { get; private set; }

        public string InviterUsername { get; private set; }

        public int RoomId { get; private set; }

        public override void Clear()
        {
            GameType = default;
            InviterUsername = default;
            RoomId = default;
        }

        public static RecieveBattleInvitationEventArgs Create(GameType gameType, string inviterUsername, int roomId)
        {
            RecieveBattleInvitationEventArgs testEvent = ReferencePool.Acquire<RecieveBattleInvitationEventArgs>();

            testEvent.GameType = gameType;
            testEvent.InviterUsername = inviterUsername;
            testEvent.RoomId = roomId;

            return testEvent;
        }
    }
}
