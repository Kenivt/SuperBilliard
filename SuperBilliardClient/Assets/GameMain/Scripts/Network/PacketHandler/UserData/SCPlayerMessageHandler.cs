using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCPlayerMessageHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCPlayerMessage;

        public override void Handle(object sender, Packet packet)
        {
            SCPlayerMessage packetImpl = (SCPlayerMessage)packet;

            GameEntry.Event.Fire(this, RecievePlayerMessageEventArgs.Create(packetImpl));
        }
    }
}