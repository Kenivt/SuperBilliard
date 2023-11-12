using GameFramework;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.Play
{
    public class PlayerRoomState : IReference
    {
        public Player player;
        public bool isReady;
        public bool isLoadGameComplete;

        public string UserName
        {
            get
            {
                if (player == null)
                {
                    return null;
                }
                return player.UserName;
            }
        }

        internal static PlayerRoomState Create(Player player)
        {
            PlayerRoomState playerRoomState = ReferencePool.Acquire<PlayerRoomState>();
            playerRoomState.player = player;
            return playerRoomState;
        }

        public void Clear()
        {
            player = null;
            isReady = false;
            isLoadGameComplete = false;
        }

        public void SendKcpPacket(Packet packet)
        {
            player.SendKcpPacket(packet);
        }

        public void SendPacket(Packet packet)
        {
            player.SendPacket(packet);
        }
    }
}
