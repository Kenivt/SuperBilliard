using System;
using System.Net;
using System.Buffers;
using System.Threading;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using System.Threading.Tasks;

namespace SuperBilliard
{
    public class KcpClient : IKcpCallback
    {
        private UdpClient _client;
        public SimpleSegManager.Kcp kcp { get; }
        public IPEndPoint EndPoint { get; set; }
        public IPEndPoint OwnEndPoint
        {
            get
            {
                IPEndPoint endPoint = _client.Client.LocalEndPoint as IPEndPoint;
                return endPoint;
            }
        }
        public KcpClient(int port) : this(port, null)
        { }
        public KcpClient(int port, IPEndPoint endPoint)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Loopback, port);
            _client = new UdpClient(iPEndPoint);
            kcp = new SimpleSegManager.Kcp(2001, this);
            this.EndPoint = endPoint;
            //间隔为10ms，快速模式
            kcp.NoDelay(1, 10, 2, 1);
        }
        public KcpClient(int port, IPEndPoint endPoint, CancellationToken recvToken)
        {
            _client = new UdpClient(port);
            kcp = new SimpleSegManager.Kcp(2001, this);
            this.EndPoint = endPoint;
            //间隔为10ms，快速模式
            kcp.NoDelay(1, 10, 2, 1);
            BeginRecvInput(recvToken);
        }
        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            _client.SendAsync(s, s.Length, EndPoint);
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
                    EndPoint = res.RemoteEndPoint;
                    kcp.Input(res.Buffer);
                }
            });
        }
        public void BeginUpdate(CancellationToken recvToken)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (recvToken.IsCancellationRequested)
                    {
                        return;
                    }
                    await Task.Delay(10);
                    UpdateKcp(DateTimeOffset.UtcNow);
                }
            });
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