using GameFramework;
using GameFramework.Event;
namespace SuperBilliard
{
    public class UpdataStorageBarEventArgs : GameEventArgs
    {
        public static int EventId => typeof(UpdataStorageBarEventArgs).GetHashCode();
        public override int Id => EventId;
        public float FillAmount { get; private set; }

        public override void Clear()
        {
            FillAmount = default;
        }
        public static UpdataStorageBarEventArgs Create(float fillAmount)
        {
            UpdataStorageBarEventArgs testEvent = ReferencePool.Acquire<UpdataStorageBarEventArgs>();
            testEvent.FillAmount = fillAmount;
            return testEvent;
        }
    }
}
