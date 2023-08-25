using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSCreateRoomHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSCreateGame;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSCreateRoom cSCreateRoom = packet as CSCreateRoom;

            if (cSCreateRoom == null)
            {
                Log.Error("CSCreateRoomHandler: cSCreateRoom is null");
                return;
            }
            if (client.Player == null)
            {
                Log.Error("CSCreateRoomHandler: client.Player is null");
                return;
            }

            //创建房间,发送确认包
            IGameRoom gameroom = GameManager.Instance.CreateGameRoom(client.Player, cSCreateRoom.GameType);
            SCCreateRoom sCCreateRoom = SCCreateRoom.Create();
            sCCreateRoom.RoomId = gameroom.RoomId;
            sCCreateRoom.GameType = gameroom.GameType;
            client.SendPacket(sCCreateRoom);

            //释放对应的packet
            ReferencePool.Release(sCCreateRoom);
        }
    }
}
