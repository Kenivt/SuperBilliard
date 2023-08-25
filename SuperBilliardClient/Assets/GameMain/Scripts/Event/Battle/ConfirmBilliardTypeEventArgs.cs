using GameMessage;
using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class ConfirmBilliardTypeEventArgs : GameEventArgs
    {
        public static int EventId => typeof(ConfirmBilliardTypeEventArgs).GetHashCode();

        public override int Id => EventId;

        public BilliardType billiardType { get; private set; }
        public override void Clear()
        {
            billiardType = BilliardType.None;
        }

        public static ConfirmBilliardTypeEventArgs Create(BilliardType billiardType)
        {
            ConfirmBilliardTypeEventArgs testEvent = ReferencePool.Acquire<ConfirmBilliardTypeEventArgs>();
            testEvent.billiardType = billiardType;
            return testEvent;
        }
    }
}
