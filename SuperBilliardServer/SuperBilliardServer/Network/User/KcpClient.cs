using System.Net;
using System.Buffers;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;

namespace SuperBilliardServer
{
    public class KcpClient : IKcpCallback
    {
        private UdpClient _client;

        public SimpleSegManager.Kcp kcp { get; }

        public IPEndPoint ownIPEndPoint
        {
            get
            {
                return (IPEndPoint)_client.Client.LocalEndPoint;
            }
        }

        /// <summary>
        /// 接收到消息的端点
        /// </summary>
        public IPEndPoint TargetEndPoint { get; set; }

        public KcpClient(IPAddress iPAddress, int port) : this(iPAddress, port, new IPEndPoint(IPAddress.Any, 0))
        {

        }

        public KcpClient(IPAddress iPAddress, int port, IPEndPoint targetEndPoint)
        {
            _client = new UdpClient(new IPEndPoint(iPAddress, port));
            kcp = new SimpleSegManager.Kcp(2001, this);
            kcp.NoDelay(1, 10, 2, 1);//fast
            TargetEndPoint = targetEndPoint;
        }

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            _client.SendAsync(s, s.Length, TargetEndPoint);
            buffer.Dispose();
        }

        /// <summary>
        /// 在Update中进行调用
        /// </summary>
        /// <param name="time"></param>
        public void UpdateKcp(in DateTimeOffset time)
        {
            kcp.Update(time);
        }

        public void Send(byte[] datagram, int bytes)
        {
            if (TargetEndPoint == null)
            {
                return;
            }
            kcp.Send(datagram.AsSpan().Slice(0, bytes));
        }

        public bool ReceiveSync(out byte[] bytes)
        {
            var (buffer, avalidLength) = kcp.TryRecv();
            if (buffer == null)
            {
                bytes = null;
                return false;
            }
            bytes = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            return true;
        }

        public async ValueTask<byte[]> ReceiveAsync()
        {
            var (buffer, avalidLength) = kcp.TryRecv();
            while (buffer == null)
            {
                await Task.Delay(10);
                (buffer, avalidLength) = kcp.TryRecv();
            }

            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            return s;
        }

        /// <summary>
        /// 开始的时候调用
        /// </summary>
        /// <param name="recvToken"></param>
        public void BeginRecvInput(CancellationToken recvToken)
        {

            Task.Run(async () =>
            {
                while (true)
                {
                    if (recvToken.IsCancellationRequested)
                    {
                        return;
                    }
                    var res = await _client.ReceiveAsync();
                    TargetEndPoint = res.RemoteEndPoint;
                    kcp.Input(res.Buffer);
                }
            });
        }
        /// <summary>
        /// 开始更新
        /// </summary>
        /// <param name="recvToken"></param>
        public void BeginUpdate(CancellationToken token)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(10);
                    if (token.IsCancellationRequested == true)
                    {
                        return;
                    }
                    kcp.Update(DateTimeOffset.UtcNow);
                }
            });
        }

        public static int GetKcpPort()
        {
            var listener1 = new TcpListener(System.Net.IPAddress.Any, 0);
            listener1.Start();
            var port1 = ((System.Net.IPEndPoint)listener1.LocalEndpoint).Port;
            listener1.Stop();
            return port1;
        }

        public static (IPAddress, int) GetKcpIpEndPoint(string ipEndPointKey)
        {
            string[] ipEndPoint = ipEndPointKey.Split('|');
            IPAddress iPAddress = IPAddress.Parse(ipEndPoint[0]);
            int port = int.Parse(ipEndPoint[1]);
            return (iPAddress, port);
        }

        public static string GetKcpIpEndPointKey(IPEndPoint iPEndPoint)
        {
            string ipEndPointKey = iPEndPoint.Address.ToString() + "|" + iPEndPoint.Port;
            return ipEndPointKey;
        }
    }
}