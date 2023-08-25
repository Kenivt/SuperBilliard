using GameFramework;
using GameFramework.Network;
using GameMessage;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SCConfirmHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.Confirm;

        public override void Handle(object sender, Packet packet)
        {
            SCConfirm sCConfirm = packet as SCConfirm;
        }
    }
}