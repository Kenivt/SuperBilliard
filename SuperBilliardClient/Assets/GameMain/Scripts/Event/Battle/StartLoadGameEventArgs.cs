using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class StartLoadGameEventArgs : GameEventArgs
    {
        public static int EventId => typeof(StartLoadGameEventArgs).GetHashCode();
        public override int Id => EventId;

        public bool IsFirstMove
        {
            get; private set;
        }

        public EnumBattle GameType
        {
            get; private set;
        }

        public string OpponentUserName
        {
            get; private set;
        }
        public int RandomSeed { get; private set; }
        public override void Clear()
        {
            GameType = EnumBattle.None;
            IsFirstMove = false;
            OpponentUserName = "default";
        }

        public static StartLoadGameEventArgs Create(EnumBattle gameType, string username, bool isFirstMove, int randomSeed)
        {
            StartLoadGameEventArgs testEvent = ReferencePool.Acquire<StartLoadGameEventArgs>();
            testEvent.GameType = gameType;
            testEvent.OpponentUserName = username;
            testEvent.IsFirstMove = isFirstMove;
            testEvent.RandomSeed = randomSeed;
            return testEvent;
        }
    }
}
