using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class HideBilliardIdTipEventArgs : GameEventArgs
    {
        public static int EventId => typeof(HideBilliardIdTipEventArgs).GetHashCode();

        public override int Id => EventId;

        public override void Clear()
        {

        }
        public static HideBilliardIdTipEventArgs Create()
        {
            HideBilliardIdTipEventArgs testEvent = ReferencePool.Acquire<HideBilliardIdTipEventArgs>();
            return testEvent;
        }
    }
}
