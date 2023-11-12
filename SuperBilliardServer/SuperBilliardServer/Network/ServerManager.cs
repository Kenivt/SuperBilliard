using System.Net;
using ServerCore.Sington;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network
{
    internal class ServerManager : SingtonBase<ServerManager>
    {
        private readonly Dictionary<string, Server> _serverDic = new Dictionary<string, Server>();

        public void AddServer(IPAddress address, int port)
        {
            string key = GetServerKey(address, port);
            lock (_serverDic)
            {
                if (_serverDic.ContainsKey(key))
                {
                    Console.WriteLine("错误，重复添加相同地址{0}的服务器", key);
                    return;
                }
                Server server = new Server(address, port);
                _serverDic[key] = server;
                server?.Init();
            }
        }

        public void RemoveServer(IPAddress address, int port)
        {
            string key = GetServerKey(address, port);
            lock (_serverDic)
            {
                if (!_serverDic.ContainsKey(key))
                {
                    Log.Error("错误，不存在地址为{0}的服务器", key);
                    return;
                }
                _serverDic.Remove(key);
            }
        }

        public static string GetServerKey(IPAddress address, int port) => string.Format("{0},{1}", address.Address, port);

        public override void Dispose()
        {
            //移除所有的服务器
            foreach (var server in _serverDic)
            {
                server.Value.Dispose();
            }
            _serverDic.Clear();
        }
    }
}