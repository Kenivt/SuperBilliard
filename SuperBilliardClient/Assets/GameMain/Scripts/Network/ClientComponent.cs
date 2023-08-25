using System;
using System.IO;
using System.Net;
using UnityEngine;
using GameMessage;
using GameFramework;
using System.Threading;
using System.Net.Sockets;
using Unity.Collections;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class ClientComponent : GameFrameworkComponent
    {
        [SerializeField] private int port = 8080;
        [SerializeField] private string ipString = "127.0.0.1";
        [SerializeField] private float _heartBeatInterval = 2f;
        public const string MainClientName = "MainClient";

        public INetworkChannelHelper NetworkChannelHelper { get; private set; } = new NetworkChannelHelper();
        public INetworkChannel networkChannel { get; private set; }

        public bool IsConnected => networkChannel.Connected;

        private float _syncInterval = 0.035f;
        private float _lastSyncTime = 0;

        public void OpenMainClient()
        {
            if (GameEntry.Network.HasNetworkChannel(MainClientName) == false)
            {
                networkChannel = GameEntry.Network.CreateNetworkChannel(MainClientName, GameFramework.Network.ServiceType.Tcp, NetworkChannelHelper);
                networkChannel.Connect(IPAddress.Parse(ipString), port);
                networkChannel.HeartBeatInterval = 2f;
            }
            else
            {
                networkChannel = GameEntry.Network.GetNetworkChannel(MainClientName);
                networkChannel.Connect(IPAddress.Parse(ipString), port);
                networkChannel.HeartBeatInterval = 2f;
            }
        }

        public void Send(Packet packet)
        {
            networkChannel.Send(packet);
        }

        //--------------------------------------------------------------------------------------------------------------
        public IPEndPoint KcpOwnEndPoint
        {
            get
            {
                if (_kcpClient == null)
                {
                    return null;
                }
                return _kcpClient.OwnEndPoint;
            }
        }

        public IPEndPoint KcpEndPoint
        {
            get
            {
                if (_kcpClient == null)
                {
                    return null;
                }
                return _kcpClient.EndPoint;
            }
            set
            {
                if (_kcpClient == null)
                {
                    return;
                }
                _kcpClient.EndPoint = value;
            }
        }

        private KcpClient _kcpClient;
        private KcpMessageHelper _kcpMessageHelper = new KcpMessageHelper();
        private MemoryStream _kcpCacheStream = new MemoryStream();
        private CancellationTokenSource _source;

        [ReadOnly, SerializeField] private bool _kcpRunning;
        public IPEndPoint RoomEndPoint
        {
            get
            {
                return _kcpClient.EndPoint;
            }
        }

        public void InitKcp()
        {
            //随机获得一个空闲端口
            int port = GetAvailableUdpPort();
            if (_kcpClient == null)
            {
                _kcpClient = new KcpClient(port);
            }
            _source = new CancellationTokenSource();
            _kcpClient.BeginRecvInput(_source.Token);

            //发送打开Kcp的消息
            string key = KcpClient.GetKcpIpEndPointKey(_kcpClient.OwnEndPoint);
            CSOpenKcp cSOpenKcp = CSOpenKcp.Create(key);
            GameEntry.Client.Send(cSOpenKcp);
        }

        public void StartKcp()
        {
            _kcpRunning = true;
        }

        public void CloseKcp()
        {
            //_source.Cancel();
            _kcpRunning = false;
        }

        public void SendKcpMessage(Packet packet, IPEndPoint end)
        {
            if (_kcpClient == null)
            {
                Log.Error("KcpClient is null");
                return;
            }
            _kcpClient.EndPoint = end;
            _kcpCacheStream.SetLength(0);
            _kcpCacheStream.Position = 0;
            _kcpMessageHelper.Serialize(packet, _kcpCacheStream);
            byte[] bytes = _kcpCacheStream.ToArray();
            _kcpClient.Send(bytes, bytes.Length);
        }

        public void SendKcpMessage(Packet packet)
        {
            if (_kcpClient == null)
            {
                Log.Error("KcpClient is null");
                return;
            }
            if (_kcpClient.EndPoint == null)
            {
                Log.Error("KcpClient EndPoint is null");
                return;
            }
            _kcpCacheStream.SetLength(0);
            _kcpCacheStream.Position = 0;
            _kcpMessageHelper.Serialize(packet, _kcpCacheStream);
            byte[] bytes = _kcpCacheStream.ToArray();
            _kcpClient.Send(bytes, bytes.Length);
        }

        public void Update()
        {
            if (_kcpClient != null)
            {
                if (UnityEngine.Time.time - _lastSyncTime > _syncInterval)
                {
                    _lastSyncTime = UnityEngine.Time.time;
                    GameEntry.Event.FireNow(this, SendSyncMessageEventArgs.Create());
                }
                _kcpClient.UpdateKcp(DateTimeOffset.UtcNow);
                try
                {
                    while (_kcpClient.ReceiveSync(out var bytes))
                    {
                        var hander = _kcpMessageHelper.DeserializePacketHeader(bytes, out object errorObj);
                        if (hander == null)
                        {
                            Log.Error("KcpClient ReceiveSync DeserializePacketHeader error: " + errorObj);
                            continue;
                        }
                        var packet = _kcpMessageHelper.DeserializePacket(hander, bytes, Constant.PacketConstant.PacketHeaderLength, out errorObj);
                        if (packet == null)
                        {
                            Log.Error("KcpClient ReceiveSync DeserializePacket error: " + errorObj);
                            continue;
                        }
                        if (_kcpMessageHelper.GetPacketHandler(hander.Id, out IPacketHandler packetHandler))
                        {
                            packetHandler.Handle(_kcpClient, packet);
                        }
                        else
                        {
                            Log.Error("KcpClient ReceiveSync Not found packet handler: " + hander.Id);
                        }
                        ReferencePool.Release(hander);
                        ReferencePool.Release(packet);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
        }

        public void ShutDown()
        {
            NetworkChannelHelper.Shutdown();
            networkChannel.Close();
        }

        private void OnDestroy()
        {
            //资源清理
            _source.Cancel();
        }

        public static int GetAvailableUdpPort()
        {
            var listener = new TcpListener(System.Net.IPAddress.Any, 0);
            listener.Start();
            var port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return port;
        }
    }
}
