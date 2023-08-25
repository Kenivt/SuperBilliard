using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class GameOverEventArgs : GameEventArgs
    {
        public static int EventId => typeof(GameOverEventArgs).GetHashCode();

        public override int Id => EventId;
        public GameMessage.GameResult GameResult { get; private set; }
        public override void Clear()
        {

        }
        public static GameOverEventArgs Create(GameMessage.GameResult gameResult)
        {
            GameOverEventArgs testEvent = ReferencePool.Acquire<GameOverEventArgs>();
            testEvent.GameResult = gameResult;
            return testEvent;
        }
    }
}
