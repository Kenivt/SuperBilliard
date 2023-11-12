using System.Net;
using System.Net.Sockets;
using SuperBilliardServer.Debug;

namespace SuperBilliardServer.Network.User
{
    public class Server : IDisposable
    {
        private Socket _socket;
        private CancellationTokenSource _source;

        public Server(IPAddress iPAddress, int port)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            _socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(iPEndPoint);
            _socket.Listen(4);
        }


        public void Init()
        {
            _source = new CancellationTokenSource();
            Task.Run(AcceptClient, _source.Token);
        }

        private async void AcceptClient()
        {
            while (true)
            {
                Socket clientSocket = await _socket.AcceptAsync();

                string clientKey = GetClientId(clientSocket);

                Client client = Client.Create(clientSocket, clientKey, this);

                if (ClientManager.Instance.HasRigisterClient(clientKey) == false)
                {
                    Log.Info("一个新的客户端注册...");
                    ClientManager.Instance.RigisterClient(clientKey, client);
                }
                else
                {
                    Log.Info("客户端已经断开连接，重新连接...");
                    ClientManager.Instance.ReconnectClient(clientKey, client);
                }

                client.BeginReceive();
            }
        }

        public static string GetClientId(Socket client)
        {
            if (client.RemoteEndPoint == null)
            {
                return string.Empty;
            }
            IPEndPoint remoteEndPoint = (IPEndPoint)client.RemoteEndPoint;
            return remoteEndPoint.Address.ToString() + ":" + remoteEndPoint.Port.ToString();
        }

        public void Dispose()
        {
            _source?.Cancel();
        }
    }
#pragma warning restore CS8600 
}