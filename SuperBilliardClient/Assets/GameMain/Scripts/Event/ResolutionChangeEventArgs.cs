using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class ResolutionChangeEventArgs : GameEventArgs
    {
        public static int EventId => typeof(ResolutionChangeEventArgs).GetHashCode();

        public override int Id => EventId;

        public float Width { get; private set; }

        public float Height { get; private set; }

        public bool IsFull { get; private set; }
        public override void Clear()
        {
            Width = 1920;
            Height = 1080;
            IsFull = false;
        }
        public static ResolutionChangeEventArgs Create(float width, float height, bool isFull = false)
        {
            ResolutionChangeEventArgs testEvent = ReferencePool.Acquire<ResolutionChangeEventArgs>();
            testEvent.Width = width;
            testEvent.Height = height;
            testEvent.IsFull = isFull;
            return testEvent;
        }
    }
}
