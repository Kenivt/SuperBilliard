using GameFramework;
using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSBilliardConfirmHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSConfirmBilliardType;
        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSBilliardTypeConfirm cSConfirmBilliardType = packet as CSBilliardTypeConfirm;
            if (cSConfirmBilliardType == null)
            {
                return;
            }
            if (client.Player == null)
            {
                return;
            }
            IGameRoom gameRoom = client.Player.GameRoom;

            if (gameRoom == null)
            {
                return;
            }
            BilliardType billiardType = BilliardType.None;

            if (cSConfirmBilliardType.CsBilliardType == BilliardType.Single)
            {
                billiardType = BilliardType.Double;

            }
            else if (cSConfirmBilliardType.CsBilliardType == BilliardType.Double)
            {
                billiardType = BilliardType.Single;
            }

            if (billiardType == BilliardType.None)
            {
                Log.Error("billiardType is None");
                return;
            }
            SCBilliardTypeConfirm sCBilliardTypeConfirm = SCBilliardTypeConfirm.Create(billiardType);
            lock (gameRoom)
            {
                gameRoom.SendMessageToAnother(client.Player, sCBilliardTypeConfirm);
            }
            ReferencePool.Release(sCBilliardTypeConfirm);
        }
    }
}
