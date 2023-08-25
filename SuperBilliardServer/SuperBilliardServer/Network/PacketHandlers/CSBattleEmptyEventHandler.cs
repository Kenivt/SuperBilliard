using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSBattleEmptyEventHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSBattleEmptyEvent;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSBattleEmptyEvent cSBattle = packet as CSBattleEmptyEvent;
            if (cSBattle == null)
            {
                return;
            }
            if (client.Player == null)
            {
                Log.Error("Error,player is invalid.");
                return;
            }

            IGameRoom gameRoom = client.Player.GameRoom;
            if (gameRoom == null)
            {
                Log.Error("Error,gameRoom is invalid.");
                return;
            }
            SCBattleEmptyEvent sCBattleEmptyEvent = SCBattleEmptyEvent.Create(cSBattle.BattleEvent);
            gameRoom.SendMessageToAnother(client.Player, sCBattleEmptyEvent);
            ReferencePool.Release(sCBattleEmptyEvent);
        }
    }
}
