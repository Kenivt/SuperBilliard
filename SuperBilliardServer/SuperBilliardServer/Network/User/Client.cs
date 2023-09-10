using GameFramework;
using Google.Protobuf;
using System.Net.Sockets;
using SuperBilliardServer.Tools;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.User
{
    public partial class Client : IReference
    {

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private Socket _socket;
        public Server Server { get; private set; }

        #region 与玩家相关的属性及字段

        public Player? Player
        {
            get => _player;
            set
            {
                _player = value;
            }
        }

        private Player? _player;

        #endregion

        #region client连接管理
        public string ClientId { get; private set; }

        public void ResetClientInfo()
        {
            ClientInfo? clientInfo = ClientManager.Instance.GetClientInfo(ClientId);
            if (clientInfo != null)
            {
                clientInfo.Reset();
            }
            else
            {
                Log.Warning("Reset客户端信息失败，客户端信息不存在...");
            }
        }
        #endregion

        #region 发送TCP消息
        public int BufferSize => _message.Buffer.Length;

        private Message _message;

        public void BeginReceive()
        {
            _socket.BeginReceive(_message.Buffer, _message.StartIndex, _message.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

        public bool SerilizePacketToMessages(Packet packet)
        {
            bool flag = false;
            lock (_message)
            {
                //防止消息过大,消息超出容量则先发送消息
                IMessage message = (IMessage)packet;
                int size = message.CalculateSize();
                if (_message.SendRemainSize < size)
                {
                    //先发送消息
                    Send();
                }

                flag = _message.SerilizePacket(packet);
            }
            return flag;
        }

        public void Send()
        {
            lock (_message)
            {
                _message.SendMessage(_socket);
            }
        }

        public void SendPacket(params Packet[] packet)
        {
            foreach (var item in packet)
            {
                SerilizePacketToMessages(item);
            }
            Send();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (_socket == null || !_socket.Connected) return;
                int len = _socket.EndReceive(ar);
                if (len == 0)
                {
                    if (_socket.Connected == false)
                    {
                        Log.Warning("有人强制退出了...");
                        _socket.Close(); // 关闭连接
                        return;
                    }
                    Log.Info("接收到无效的消息...");
                    return;
                }
                _message.DeserializeBufferData(len, ReadBufferCallback);
                BeginReceive();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "有人强制退出了...{0}", _socket.EndReceive(ar));
            }
        }

        private void ReadBufferCallback(object? sender, Packet e)
        {
            PacketManager.Instance.ExecutePackHandle(this, this, e);
        }

        #endregion

        public static Client Create(Socket socket, string clientid, Server server)
        {
            Client client = ReferencePool.Acquire<Client>();

            client._socket = socket;

            client._message = Message.Create();
            client.ClientId = clientid;
            client.Server = server;

            return client;
        }

        public void Clear()
        {
            _socket?.Close();
            _socket = null;
            Server = null;
            ClientId = string.Empty;

            // 退出房间
            if (_player != null)
            {
                if (_player.GameRoom != null)
                {
                    _player.GameRoom.Leave(_player);
                }
                PlayerManager.Instance.UnrigisterPlayer(_player.UserName);
                _player = null;
            }

            ReferencePool.Release(_message);
            _message = null;

            CloseKcp();
            Kcp = null;
            KcpSource = null;
        }
    }
}