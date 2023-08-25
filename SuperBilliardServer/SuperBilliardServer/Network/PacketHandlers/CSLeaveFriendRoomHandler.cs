using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Play;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSLeaveFriendRoomHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSLeaveFriendRoom;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSLeaveRoom csPacket = (CSLeaveRoom)packet;

            if (client.Player == null)
            {
                return;
            }

            //发送离开房间的消息
            SCLeaveRoom sCLeaveRoom = SCLeaveRoom.Create(client.Player.UserName);
            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Warning("玩家没有在房间中....");
                return;
            }
            gameRoom.Broadcast(sCLeaveRoom);

            client.Player.State = PlayerStatus.Idle;
            if (gameRoom.Leave(client.Player) == false)
            {
                Log.Warning("离开房间失败");
            }
            ReferencePool.Release(sCLeaveRoom);
        }
    }
}
