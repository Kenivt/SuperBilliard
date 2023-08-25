using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSSyncSoundHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSSyncSound;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSSyncSound cSSyncSound = (CSSyncSound)packet;

            if (client.Player == null)
            {
                Log.Error("玩家还未初始化，无法同步音效");
                return;
            }

            IGameRoom gameRoom = client.Player.GameRoom;

            if (gameRoom == null)
            {
                Log.Error("玩家还未加入房间，无法同步音效");
                return;
            }
            Vector3 pos = new Vector3(cSSyncSound.Position.X, cSSyncSound.Position.Y, cSSyncSound.Position.Z);
            SCSyncSound sCSyncSound = SCSyncSound.Create(cSSyncSound.SoundId, cSSyncSound.Volumn, pos);
            gameRoom.SendMessageToAnother(client.Player, sCSyncSound);
            ReferencePool.Release(sCSyncSound);
        }
    }
}
