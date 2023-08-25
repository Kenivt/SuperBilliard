using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class GameStartEventArgs : GameEventArgs
    {
        public static int EventId => typeof(GameStartEventArgs).GetHashCode();

        public override int Id => EventId;

        public float DelayStartTime { get; private set; }

        public override void Clear()
        {
            DelayStartTime = 0f;
        }
        public static GameStartEventArgs Create(float delayStartTime)
        {
            GameStartEventArgs testEvent = ReferencePool.Acquire<GameStartEventArgs>();
            testEvent.DelayStartTime = delayStartTime;
            return testEvent;
        }
    }
}
