using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class BilliardGoalEventArgs : GameEventArgs
    {
        public static int EventId => typeof(BilliardGoalEventArgs).GetHashCode();
        public override int Id => EventId;
        public IBilliard Billiard { get; private set; }
        public override void Clear()
        {
            Billiard = null;
        }
        public static BilliardGoalEventArgs Create(IBilliard billiard)
        {
            BilliardGoalEventArgs testEvent = ReferencePool.Acquire<BilliardGoalEventArgs>();
            testEvent.Billiard = billiard;
            return testEvent;
        }
    }
}
