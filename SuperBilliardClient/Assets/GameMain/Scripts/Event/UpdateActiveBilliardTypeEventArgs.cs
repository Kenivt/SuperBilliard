using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class UpdateActiveBilliardTypeEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdateActiveBilliardTypeEventArgs).GetHashCode();

        public override int Id => EventId;
        public bool IsOwnTurn { get; private set; }
        public SnokkerBilliardType BilliardType { get; private set; }
        public override void Clear()
        {
            BilliardType = SnokkerBilliardType.Red;
            IsOwnTurn = true;
        }
        public static UpdateActiveBilliardTypeEventArgs Create(bool isOwn, SnokkerBilliardType billiardType)
        {
            UpdateActiveBilliardTypeEventArgs testEvent = ReferencePool.Acquire<UpdateActiveBilliardTypeEventArgs>();
            testEvent.BilliardType = billiardType;
            testEvent.IsOwnTurn = isOwn;
            return testEvent;
        }
    }
}
