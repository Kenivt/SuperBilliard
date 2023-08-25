using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Constant;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSBilliardSyncHandler : PacketSyncHandler
    {
        public override int Id => PacketTypeId.CSBilliardSync;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSBilliardSync ballSyncPacket = packet as CSBilliardSync;
            if (ballSyncPacket == null)
            {
                Log.Error("ballSyncPacket is null on{0}", this.GetType().ToString());
                return;
            }
            SCBilliardSync sCBallClubSync = ReferencePool.Acquire<SCBilliardSync>();

            foreach (var item in ballSyncPacket.BallMessages)
            {
                sCBallClubSync.BallMessages.Add(BallMessage.Create(item.BallId,
                    new Vector3(item.Position.X, item.Position.Y, item.Position.Z)
                    , new Vector3(item.Rotation.X, item.Rotation.Y, item.Rotation.Z)));
            }
            if (client.Player == null)
            {
                Log.Info("client.Player is null on{0}", this.GetType().ToString());
                return;
            }
            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Info("client.Player.GameRoom is null on{0}", this.GetType().ToString());
                return;
            }
            lock (gameRoom)
            {
                gameRoom.SendKcpMessageToAnother(client.Player, sCBallClubSync);
            }
            ReferencePool.Release(sCBallClubSync);
        }
    }
}
