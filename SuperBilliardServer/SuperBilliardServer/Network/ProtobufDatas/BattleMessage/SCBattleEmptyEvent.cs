using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCBattleEmptyEvent : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCBattleEmptyEvent;

        public override void Clear()
        {
            BattleEvent = BattleEmptyEvent.BattleNone;
        }

        public static SCBattleEmptyEvent Create(BattleEmptyEvent battleEmptyEvent)
        {
            SCBattleEmptyEvent packet = ReferencePool.Acquire<SCBattleEmptyEvent>();

            packet.BattleEvent = battleEmptyEvent;
            return packet;
        }
    }
}
