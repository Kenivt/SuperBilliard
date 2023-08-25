using ServerCore;
using System.Net;
using GameMessage;
using ServerCore.Sington;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network;
using SuperBilliardServer.Network.Play;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer
{
    /// <summary>
    /// 模仿ET框架做的单例管理,因为之前的代码太乱了,所以对于整体的框架进行了重构
    /// </summary>
    public static class ServerBase
    {
        private static readonly Dictionary<Type, ISington> _singtonDic = new Dictionary<Type, ISington>();

        private static readonly Queue<ISington> _updateQueue = new Queue<ISington>();

        private static Server _server;

        public static void Init()
        {
            AddSington<MainThreadSyncContext>();
            AddSington<ClientManager>();
            AddSington<GameManager>();
            AddSington<PacketManager>();
            AddSington<SqlManager>();
            AddSington<PlayerManager>();

            //注册一下...
            GameManager.Instance.RigisterGameGroup(new MatchGameGroup(GameType.FancyMatch));
            GameManager.Instance.RigisterGameGroup(new MatchGameGroup(GameType.SnookerMatch));

            GameManager.Instance.RigisterGameGroup(new FirendGameGroup(GameType.FancyFriend));
            GameManager.Instance.RigisterGameGroup(new FirendGameGroup(GameType.SnookerFriend));

            //注册对应的Sql
            SqlManager.Instance.RigisterSqlHandler<IFriendSqlHandler>(new FriendSqlHandler());
            SqlManager.Instance.RigisterSqlHandler<ILoginSqlHandler>(new LogicSqlHandler());
            SqlManager.Instance.RigisterSqlHandler<IPlayerMessageSqlHandler>(new PlayerMessageSqlHandler());
            //创建一个服务器
            _server = new Server(IPAddress.Parse("127.0.0.1"), 8080);
            _server.Init();
        }

        private static void AddSington<T>() where T : ISington, new()
        {
            T instance = new T();
            Type type = typeof(T);
            if (_singtonDic.ContainsKey(type))
            {
                return;
            }

            instance.Rigister();
            _singtonDic.Add(type, instance);

            if (instance is IUpdateSington)
            {
                _updateQueue.Enqueue(instance);
            }
        }

        public static void Update()
        {
            int count = _updateQueue.Count;
            while (count > 0)
            {
                ISington sington = _updateQueue.Dequeue();
                count--;
                if (sington.IsDisposed)
                {
                    continue;
                }

                if (sington is not IUpdateSington update)
                {
                    continue;
                }
                _updateQueue.Enqueue(sington);
                try
                {
                    update.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static void ShutDown()
        {
            foreach (var item in _singtonDic)
            {
                item.Value.ShutDown();
            }
        }
    }
}
