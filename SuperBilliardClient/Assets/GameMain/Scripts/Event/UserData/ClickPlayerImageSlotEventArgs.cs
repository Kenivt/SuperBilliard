using UnityEngine;
using GameFramework;
using GameFramework.Event;

namespace SuperBilliard
{
    public class ClickPlayerImageSlotEventArgs : GameEventArgs
    {
        public static int EventId => typeof(ClickPlayerImageSlotEventArgs).GetHashCode();

        public override int Id => EventId;

        public int PlayerImageID { get; private set; }
        public Sprite PlayerImageSprite { get; private set; }

        public override void Clear()
        {
            PlayerImageID = 1;
            PlayerImageSprite = null;
        }

        public static ClickPlayerImageSlotEventArgs Create(int id, Sprite playerimage)
        {
            ClickPlayerImageSlotEventArgs testEvent = ReferencePool.Acquire<ClickPlayerImageSlotEventArgs>();
            testEvent.PlayerImageID = id;
            testEvent.PlayerImageSprite = playerimage;

            return testEvent;
        }
    }
}
