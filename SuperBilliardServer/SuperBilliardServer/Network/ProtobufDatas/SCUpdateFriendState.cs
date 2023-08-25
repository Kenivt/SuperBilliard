using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCUpdateFriendState : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCUpdateFriendState;

        public override void Clear()
        {
            username_ = "default";
            status_ = PlayerStatus.PlayerStausNone;
        }

        public static SCUpdateFriendState Create(string username, PlayerStatus status)
        {
            SCUpdateFriendState packet = ReferencePool.Acquire<SCUpdateFriendState>();
            packet.username_ = username;
            packet.status_ = status;
            return packet;
        }
    }
}
