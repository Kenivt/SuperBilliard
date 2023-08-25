using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSSetBIlliardActiveHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSSetBilliardState;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSSetBilliardState cSSetBilliardActive = (CSSetBilliardState)packet;

            if (client.Player == null)
            {
                return;
            }
            if (client.Player.GameRoom == null)
            {
                return;
            }

            SCSetBilliardState sCSetBilliardActive = SCSetBilliardState.Create(cSSetBilliardActive.BilliardId,
                cSSetBilliardActive.Active, cSSetBilliardActive.PhysicsIsOpen);
            IGameRoom gameRoom = client.Player.GameRoom;
            gameRoom.SendMessageToAnother(client.Player, sCSetBilliardActive);
            ReferencePool.Release(sCSetBilliardActive);
        }
    }
}
