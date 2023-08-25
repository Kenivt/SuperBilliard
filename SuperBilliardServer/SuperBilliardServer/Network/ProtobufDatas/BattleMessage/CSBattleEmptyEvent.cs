using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSBattleEmptyEvent : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSBattleEmptyEvent;

        public override void Clear()
        {
            BattleEvent = BattleEmptyEvent.BattleNone;
        }
    }
}
