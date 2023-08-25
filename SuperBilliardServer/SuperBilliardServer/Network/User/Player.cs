using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.Play;

namespace SuperBilliardServer.Network.User
{
    public class Player
    {
        public Client Client
        {
            get
            {
                return _client;
            }
        }

        public string UserName { get; private set; }

        public PlayerStatus State
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status == value)
                {
                    return;
                }
                _status = value;
                //向所有的好友发送状态更新的事件...
                var packet = SCUpdateFriendState.Create(UserName, value);
                playerMessage.SendPacketToAllOnlineFriend(packet);
                ReferencePool.Release(packet);
            }
        }

        public IGameRoom GameRoom { get; set; }

        public PlayerMessage playerMessage { get; set; }
        private Client _client;
        private PlayerStatus _status;

        public Player(Client client, string userName)
        {
            playerMessage = new PlayerMessage();
            _client = client;
            UserName = userName;
            State = PlayerStatus.Idle;
        }

        public void AddOnlineFriend(string username)
        {
            playerMessage.AddOnlineFriend(username);
        }

        public void RemoveOnlineFriend(string username)
        {
            playerMessage.RemoveOnlineFriend(username);
        }

        public void SendPacket(params Packet[] packet)
        {
            foreach (var item in packet)
            {
                _client.SerilizePacketToMessages(item);
            }
            _client.Send();
        }

        public void OnEnterGameRoom(IGameRoom gameRoom)
        {
            GameRoom = gameRoom;
            State = PlayerStatus.Watting;
        }

        public void OnLeaveGameRoom()
        {
            State = PlayerStatus.Idle;
            GameRoom = null;
        }

        public void SendKcpPacket(Packet packet)
        {
            Client.SendKcpPacket(packet);
        }
    }
}