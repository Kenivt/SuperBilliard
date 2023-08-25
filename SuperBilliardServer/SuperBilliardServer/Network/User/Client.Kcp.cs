using System.Net;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.User
{
    public partial class Client
    {
        public KcpClient Kcp { get; private set; }

        public CancellationTokenSource KcpSource { get; set; }

        public MemoryStream _cacheStream = new MemoryStream();

        public IPEndPoint KcpEndPoint
        {
            get
            {
                if (Kcp == null)
                {
                    return null;
                }
                return Kcp.TargetEndPoint;
            }
        }

        public void OpenKcp(string kcpKey)
        {
            string[] words = kcpKey.Split('|');
            if (words.Length != 2)
            {
                Log.Error("KcpIpEnd is not valid, can't join game room.");
                return;
            }

            IPAddress iPAddress = IPAddress.Parse(words[0]);
            int port = Convert.ToInt32(words[1]);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            if (Kcp == null)
            {
                Kcp = new KcpClient(IPAddress.Loopback, KcpClient.GetKcpPort(), iPEndPoint);
            }
            else
            {
                Kcp.TargetEndPoint = iPEndPoint;
            }
            if (KcpSource == null)
            {
                KcpSource = new CancellationTokenSource();
            }
            else
            {
                if (KcpSource.TryReset() == false)
                {
                    KcpSource.Cancel();
                    KcpSource = new CancellationTokenSource();
                }
            }
            Kcp.BeginUpdate(KcpSource.Token);
            Kcp.BeginRecvInput(KcpSource.Token);
            ReceiveKcpClient(KcpSource.Token);
        }

        public void CloseKcp()
        {
            if (KcpSource != null)
                KcpSource.Cancel();
        }

        public void SendKcpPacket(Packet packet)
        {
            if (Kcp == null)
            {
                return;
            }
            _cacheStream.Position = 0;
            _cacheStream.SetLength(0);
            PacketManager.Instance.Serialize(packet, _cacheStream);
            Kcp.Send(_cacheStream.ToArray(), (int)_cacheStream.Length);
        }

        private void ReceiveKcpClient(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    byte[] bytes = await Kcp.ReceiveAsync();
                    if (bytes == null)
                    {
                        Log.Error("bytes is null.");
                        return;
                    }
                    IPEndPoint targetEnd = Kcp.TargetEndPoint;

                    PacketManager packetManager = PacketManager.Instance;
                    PacketHeaderBase packetHeaderBase = packetManager.DeserializePacketHeader(bytes);
                    Packet packet = packetManager.DeserializePacket(packetHeaderBase, bytes, 5);
                    if (packet == null)
                    {
                        Log.Error("packet is invalid.");
                        return;
                    }
                    ReferencePool.Release(packetHeaderBase);

                    PacketManager.Instance.ExecutePackHandle(this, this, packet);
                }
            });
        }
    }
}
