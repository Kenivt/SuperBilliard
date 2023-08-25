using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCSyncSoundHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCSyncSound;

        public override void Handle(object sender, Packet packet)
        {
            SCSyncSound sCSyncSound = (SCSyncSound)packet;
            EnumSound enumSound = (EnumSound)sCSyncSound.SoundId;
            GameEntry.Sound.PlaySound(enumSound, null, sCSyncSound.Volumn);
        }
    }
}