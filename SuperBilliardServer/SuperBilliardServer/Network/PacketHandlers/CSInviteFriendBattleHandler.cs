using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class CSInviteFriendBattleHandler : PacketSyncHandler
    {
        public override int Id => Constant.PacketTypeId.CSInviteFriendBattle;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSInviteFriendBattle csPacket = (CSInviteFriendBattle)packet;

            string targetUserName = csPacket.InviteeUserName;
            Player targetPlayer = PlayerManager.Instance.GetPlayer(targetUserName);

            //如果对方在线且空闲，才发送邀请
            if (targetPlayer.State == PlayerStatus.Idle)
            {
                SCInviteFriendBattle scPacket = SCInviteFriendBattle.
                Create(csPacket.GameType, client.Player.UserName, csPacket.RoomId);

                targetPlayer.SendPacket(scPacket);

                //释放对应的packet
                ReferencePool.Release(scPacket);
            }
        }
    }
}
