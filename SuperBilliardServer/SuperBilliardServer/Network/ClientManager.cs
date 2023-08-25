using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using System.Collections.Concurrent;
using ServerCore.Sington;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network
{
    public enum ClientState
    {
        None,
        Connected,
        Disconnected,
    }

    public class ClientInfo : IReference
    {
        public Client? Client { get; private set; }
        public ClientState clientState { get; set; }
        public long lastHeartBeatTime { get; set; }
        public string key { get; private set; }

        public bool IsHeartBeatTimeOut(float time)
        {
            return time - lastHeartBeatTime > 10;
        }
        public static ClientInfo Create(string key, Client client)
        {
            ClientInfo clientState = ReferencePool.Acquire<ClientInfo>();
            clientState.Client = client;
            clientState.key = key;
            clientState.lastHeartBeatTime = DateTime.UtcNow.Ticks;
            clientState.clientState = ClientState.Connected;
            return clientState;
        }
        public void Clear()
        {
            Client = null;
            lastHeartBeatTime = -1;
            key = null;
            clientState = ClientState.None;
        }

        public void Reset()
        {
            lastHeartBeatTime = DateTime.UtcNow.Ticks;
            clientState = ClientState.Connected;
        }
    }

    public class ClientManager : SingtonBase<ClientManager>, IUpdateSington
    {
        private ConcurrentDictionary<string, ClientInfo> _rigisterClientDic;
        private ConcurrentDictionary<string, ClientInfo> _connectClientDic;
        private ConcurrentDictionary<string, ClientInfo> _disConnectClientDic;
        public int RigisterClientCount => _rigisterClientDic.Count;
        public int ConnectClientCount => _connectClientDic.Count;

        public ClientManager()
        {
            _rigisterClientDic = new ConcurrentDictionary<string, ClientInfo>();
            _connectClientDic = new ConcurrentDictionary<string, ClientInfo>();
            _disConnectClientDic = new ConcurrentDictionary<string, ClientInfo>();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="key">对应的Key</param>
        /// <param name="client">对应的客户端</param>
        public bool RigisterClient(string key, Client client)
        {
            if (_rigisterClientDic.ContainsKey(key))
            {
                Log.Info("已经有对应的Key的客户端了...");
                return false;
            }
            ClientInfo clientState = ClientInfo.Create(key, client);
            _rigisterClientDic.TryAdd(key, clientState);
            _connectClientDic.TryAdd(key, _rigisterClientDic[key]);
            return true;
        }

        /// <summary>
        /// 取消注册客户端
        /// </summary>
        /// <param name="key">对应的key</param>
        public bool UnrigisterClient(string key)
        {
            if (_rigisterClientDic.ContainsKey(key))
            {
                if (_rigisterClientDic.TryRemove(key, out ClientInfo clientInfo1))
                {
                    if (clientInfo1.Client != null && clientInfo1.Client.Player != null)
                    {
                        //如果是登录状态，就执行登出操作
                        SqlManager.Instance.GetSqlHandler<ILoginSqlHandler>().Unlogin(clientInfo1.Client.Player.UserName);
                    }
                }
                _connectClientDic.TryRemove(key, out ClientInfo clientInfo2);
                _disConnectClientDic.TryRemove(key, out ClientInfo clientInfo3);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 客户端重新连接
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool ReconnectClient(string key, Client client)
        {
            //在注册列表中存在，但是在连接列表中不存在
            if (_disConnectClientDic.ContainsKey(key))
            {
                if (_disConnectClientDic.Remove(key, out ClientInfo clientInfo))
                {
                    if (clientInfo == null || clientInfo.Client == null)
                    {
                        Log.Warning("disConnect客户端信息为空...");
                        return false;
                    }
                    //回收客户端
                    ReferencePool.Release(clientInfo.Client);
                    ReferencePool.Release(clientInfo);
                    //创建新的客户端
                    ClientInfo newClientInfo = ClientInfo.Create(key, client);
                    newClientInfo.clientState = ClientState.Connected;
                    _rigisterClientDic.AddOrUpdate(key, newClientInfo, (key, value) => newClientInfo);
                    _connectClientDic.AddOrUpdate(key, newClientInfo, (key, value) => newClientInfo);
                    return true;
                }
                else
                {
                    Log.Warning("回收客户端失败...");
                }
            }
            return false;
        }

        /// <summary>
        /// 得到所有客户端的信息
        /// </summary>
        public ClientInfo? GetClientInfo(string key)
        {
            if (_rigisterClientDic.ContainsKey(key))
            {
                return _rigisterClientDic[key];
            }
            return null;
        }

        public bool HasRigisterClient(string key)
        {
            return _rigisterClientDic.ContainsKey(key);
        }

        private void CheckClientState(ClientInfo clientInfo, Queue<string> removeQueue, long time)
        {
            if (clientInfo == null)
            {
                return;
            }
            long lastHeartBeatTime;
            long now;

            lastHeartBeatTime = clientInfo.lastHeartBeatTime;
            now = DateTime.UtcNow.Ticks;
            TimeSpan timeSpan = new TimeSpan(now - lastHeartBeatTime);

            if (timeSpan.TotalMilliseconds > time)
            {
                clientInfo.clientState = ClientState.Disconnected;
                removeQueue.Enqueue(clientInfo.key);
            }
        }

        public bool UnrigisterAllClient()
        {
            foreach (var item in _rigisterClientDic)
            {
                UnrigisterClient(item.Key);
            }
            return true;
        }

        public void Update()
        {
            CheckAllClientConnectState();
        }
        /// <summary>
        /// 检查所有客户端的连接状态
        /// </summary>
        private void CheckAllClientConnectState()
        {
            Queue<string> disConnectQueue = new Queue<string>();
            Queue<string> unrigisterQueue = new Queue<string>();

            disConnectQueue.Clear();
            unrigisterQueue.Clear();

            foreach (var item in _connectClientDic)
            {
                CheckClientState(item.Value, disConnectQueue, Constant.ServerConstant.DisConnnectDataLineTime);
            }

            while (disConnectQueue.Count > 0)
            {
                string key = disConnectQueue.Dequeue();
                if (_connectClientDic.TryRemove(key, out ClientInfo value))
                {
                    Log.Info("IP为:" + key + "解除连接...");
                    _disConnectClientDic.TryAdd(key, value);
                }
            }

            foreach (var item in _disConnectClientDic)
            {
                CheckClientState(item.Value, unrigisterQueue, Constant.ServerConstant.UnrigisterDeadLineTime);
            }

            while (unrigisterQueue.Count > 0)
            {
                string key = unrigisterQueue.Dequeue();
                if (_disConnectClientDic.TryRemove(key, out ClientInfo value))
                {

                }

                if (_rigisterClientDic.TryRemove(key, out ClientInfo clientInfo1))
                {
                    if (clientInfo1.Client != null && clientInfo1.Client.Player != null)
                    {
                        Log.Info("IP为:" + key + "完全退出...");
                        SqlManager.Instance.GetSqlHandler<ILoginSqlHandler>().Unlogin(clientInfo1.Client.Player.UserName);
                    }
                    ReferencePool.Release(clientInfo1.Client);
                }
            }
        }
    }
}