using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class PlaceWhiteTipEventArgs : GameEventArgs
    {
        public static int EventId => typeof(PlaceWhiteTipEventArgs).GetHashCode();

        public override int Id => EventId;
        public bool IsShow { get; private set; }
        public override void Clear()
        {

        }
        public static PlaceWhiteTipEventArgs Create(bool isShow)
        {
            PlaceWhiteTipEventArgs testEvent = ReferencePool.Acquire<PlaceWhiteTipEventArgs>();
            testEvent.IsShow = isShow;
            return testEvent;
        }
    }
}
