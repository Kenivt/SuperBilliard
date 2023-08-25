using GameMessage;
using GameFramework.Network;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class SCTurnAnalysisHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCTurnAnalysis;

        public override void Handle(object sender, Packet packet)
        {
            SCTurnAnalysis pac = (SCTurnAnalysis)packet;

            GameEntry.Event.Fire(this, AllBilliardStopEventArgs.Create(pac.FristCollideBIlliardId));
        }
    }
}