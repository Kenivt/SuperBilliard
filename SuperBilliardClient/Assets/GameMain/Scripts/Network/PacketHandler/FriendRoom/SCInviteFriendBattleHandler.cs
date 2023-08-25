using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCInviteFriendBattleHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCInviteFriendBattle;

        public override void Handle(object sender, Packet packet)
        {
            var scIfb = (SCInviteFriendBattle)packet;

            GameEntry.Event.Fire(this, RecieveBattleInvitationEventArgs.Create(scIfb.GameType, scIfb.InviteeUsername, scIfb.RoomId));
        }
    }
}