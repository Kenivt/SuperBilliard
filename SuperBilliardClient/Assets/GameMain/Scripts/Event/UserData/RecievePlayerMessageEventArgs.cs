using GameFramework;
using GameFramework.Event;
using GameMessage;

namespace SuperBilliard
{
    public class RecievePlayerMessageEventArgs : GameEventArgs
    {
        public static int EventId => typeof(RecievePlayerMessageEventArgs).GetHashCode();

        public override int Id => EventId;

        public string UserName
        {
            get; private set;
        }

        public string NickName { get; private set; }
        public int Level { get; private set; }
        public int IconId { get; private set; }
        public string Description { get; private set; }

        public int FaceID { get; private set; }
        public int KitID { get; private set; }
        public int BodyID { get; private set; }
        public int HairID { get; private set; }

        public override void Clear()
        {
            NickName = default;
            Level = default;
            IconId = default;
            Description = default;
            FaceID = 1;
            KitID = 1;
            BodyID = 1;
            HairID = 1;
        }

        public static RecievePlayerMessageEventArgs Create(SCPlayerMessage sCPlayerMessage)
        {
            RecievePlayerMessageEventArgs testEvent = ReferencePool.Acquire<RecievePlayerMessageEventArgs>();
            testEvent.UserName = sCPlayerMessage.PmUserName;
            testEvent.NickName = sCPlayerMessage.PmSnikName;
            testEvent.Level = sCPlayerMessage.PmLevel;
            testEvent.IconId = sCPlayerMessage.PmIconId;
            testEvent.Description = sCPlayerMessage.PmDescription;
            testEvent.FaceID = sCPlayerMessage.PmFaceId;
            testEvent.BodyID = sCPlayerMessage.PmBodyId;
            testEvent.KitID = sCPlayerMessage.PmKitId;
            testEvent.HairID = sCPlayerMessage.PmHairId;
            return testEvent;
        }
    }
}
