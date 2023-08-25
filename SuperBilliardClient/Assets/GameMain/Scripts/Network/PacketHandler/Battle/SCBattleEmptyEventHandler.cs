using GameFramework.Network;
using GameMessage;

namespace SuperBilliard
{
    public class SCBattleEmptyEventHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCBattleEmptyEvent;

        public override void Handle(object sender, Packet packet)
        {
            SCBattleEmptyEvent packetImpl = (SCBattleEmptyEvent)packet;
            switch (packetImpl.BattleEvent)
            {
                case BattleEmptyEvent.StopTimeCount:
                    GameEntry.Event.Fire(this, StopTimeCountEventArgs.Create());
                    break;
            }
        }
    }
}