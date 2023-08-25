using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class CueStorageEventArgs : GameEventArgs
    {
        public static int EventId => typeof(CueStorageEventArgs).GetHashCode();

        public override int Id => EventId;
        public float FillAmount { get; private set; }
        /// <summary>
        /// 注意偏移量是正整数
        /// </summary>
        public float MaxOffset { get; private set; }

        public override void Clear()
        {
            MaxOffset = 0;
            FillAmount = 0;
        }
        public static CueStorageEventArgs Create(float FillAmount, float MaxOffset = 3f)
        {
            CueStorageEventArgs testEvent = ReferencePool.Acquire<CueStorageEventArgs>();
            testEvent.FillAmount = FillAmount;
            testEvent.MaxOffset = MaxOffset;
            return testEvent;
        }
    }
}
