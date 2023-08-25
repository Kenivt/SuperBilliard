using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class ShowBilliardIdTipEventArgs : GameEventArgs
    {
        public static int EventId => typeof(ShowBilliardIdTipEventArgs).GetHashCode();
        public override int Id => EventId;

        public BilliardData BilliardData { get; private set; }

        public override void Clear()
        {
            BilliardData = null;
        }

        public static ShowBilliardIdTipEventArgs Create(BilliardData battleType)
        {
            ShowBilliardIdTipEventArgs testEvent = ReferencePool.Acquire<ShowBilliardIdTipEventArgs>();
            testEvent.BilliardData = battleType;
            return testEvent;
        }
    }
}
