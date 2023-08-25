using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class UpdatePlayerScoreEvenrArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdatePlayerScoreEvenrArgs).GetHashCode();

        public override int Id => EventId;

        public bool IsOwn { get; private set; }
        public int Score { get; private set; }

        public override void Clear()
        {
            IsOwn = false;
            Score = 0;
        }
        public static UpdatePlayerScoreEvenrArgs Create(bool isOwn, int score)
        {
            UpdatePlayerScoreEvenrArgs testEvent = ReferencePool.Acquire<UpdatePlayerScoreEvenrArgs>();
            testEvent.Score = score;
            testEvent.IsOwn = isOwn;
            return testEvent;
        }
    }
}
