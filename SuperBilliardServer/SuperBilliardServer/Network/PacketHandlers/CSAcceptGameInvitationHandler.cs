using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSAcceptGameInvitationHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSAcceptGameInvitation;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSAcceptGameInvitation csPacket = (CSAcceptGameInvitation)packet;

            if (GameManager.Instance.GetGameRoom(csPacket.GameType,
                csPacket.RoomId, out IGameRoom gameroom))
            {
                SCInviteeEnterRoom scPacket = SCInviteeEnterRoom.Create(csPacket.GameType, client.Player.UserName);

                gameroom.Enter(client.Player);
                gameroom.SendMessageToAnother(client.Player, scPacket);
                bool isready = false;
                foreach (var item in gameroom.PlayerRoomStates)
                {
                    if (item != null && item.UserName == csPacket.InviterUserName)
                    {
                        isready = item.isReady; break;
                    }
                }
                SCAcceptGameInvitation scInvation = SCAcceptGameInvitation.Create(csPacket.GameType,
                    csPacket.InviterUserName, isready, csPacket.OwnUsername, InvitationResult.Success);
                //回调信息
                client.SendPacket(scInvation);

                ReferencePool.Release(scPacket);

                ReferencePool.Release(scInvation);
            }
        }
    }
}