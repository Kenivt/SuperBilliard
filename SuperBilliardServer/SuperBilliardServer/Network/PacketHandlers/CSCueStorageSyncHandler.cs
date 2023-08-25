using GameFramework;
using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.User;


namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSCueStorageSyncHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSCueStorageSync;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSCueStorageSync cSCueStorageSync = packet as CSCueStorageSync;
            if (cSCueStorageSync == null)
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
            Vector3 dir = new Vector3(cSCueStorageSync.DirX, cSCueStorageSync.DirY, cSCueStorageSync.DirZ);
            Log.Debug("dir: {0}", dir);
            SCCueStorageSync sCCueStorageSync = SCCueStorageSync.Create(cSCueStorageSync.FillAmount, dir);
            gameRoom.SendMessageToAnother(client.Player, sCCueStorageSync);
            ReferencePool.Release(sCCueStorageSync);
        }
    }
}
