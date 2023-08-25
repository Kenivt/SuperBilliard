using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCStartTurnHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCStartTurn;

        public override void Handle(object sender, Packet packet)
        {
            SCStartTurn scStartTurn = packet as SCStartTurn;
            if (scStartTurn == null)
            {
                return;
            }
            GameEntry.Event.Fire(this, StartTurnEventArgs.Create(scStartTurn.IsPlacewhite));
        }
    }
}