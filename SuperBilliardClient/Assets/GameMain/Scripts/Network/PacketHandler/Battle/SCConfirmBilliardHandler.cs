using GameMessage;
using GameFramework.Network;
using GameFramework;

namespace SuperBilliard
{
    public class SCConfirmBilliardHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCConfirmBilliardType;

        public override void Handle(object sender, Packet packet)
        {
            SCBilliardTypeConfirm sCConfirm = packet as SCBilliardTypeConfirm;
            GameEntry.Event.FireNow(this, ConfirmBilliardTypeEventArgs.Create(sCConfirm.ScBilliardType));
        }
    }
}