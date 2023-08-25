using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCSyncSound : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCSyncSound;

        public override void Clear()
        {
            SoundId = 0;
            Volumn = 1;
            ReferencePool.Release(Position);
            Position = null;
        }

        public static SCSyncSound Create(int soundId, float volumn, Vector3 pos)
        {
            SCSyncSound packet = ReferencePool.Acquire<SCSyncSound>();
            packet.SoundId = soundId;
            packet.Volumn = volumn;
            packet.Position = Vector3Mess.Create(pos);
            return packet;
        }
    }
}