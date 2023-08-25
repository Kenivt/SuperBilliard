using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSCueSyncHandler : PacketSyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSCueSync;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSCueSync cSCueSync = packet as CSCueSync;
            if (cSCueSync == null)
            {
                return;
            }
            if (client.Player == null)
            {
                Log.Error("client.Player is null on");
                return;
            }
            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Error("client.Player.GameRoom is null on{0}");
                return;
            }
            Vector3 vector3 = new Vector3(cSCueSync.Position.X, cSCueSync.Position.Y, cSCueSync.Position.Z);
            SCCueSync sCCueSync = SCCueSync.Create(vector3, cSCueSync.AngltY);
            gameRoom.SendMessageToAnother(client.Player, sCCueSync);
            ReferencePool.Release(sCCueSync);
        }
    }
}
