using GameFramework;
using System.Net.Sockets;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network;
using SuperBilliardServer.Constant;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Tools
{
    public class Message : IReference
    {
        public byte[] Buffer { get; private set; } = new byte[PacketConstant.BufferSize];
        private MemoryStream _memoryStream = new MemoryStream(PacketConstant.SendMemoryStreamSize);

        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
            private set
            {
                _startIndex = value;
            }
        }
        private int _startIndex = 0;
        public int RemainSize => Buffer.Length - StartIndex;
        public int SendRemainSize => (int)_memoryStream.Length;

        public void DeserializeBufferData(int dataLength, EventHandler<Packet> readbufferCallback)
        {
            StartIndex += dataLength;
            //不够消息的长度
            if (StartIndex <= Constant.PacketConstant.PacketHeaderLength)
            {
                return;
            }
            do
            {
                PacketHeaderBase cSPacketHeader = PacketManager.Instance.DeserializePacketHeader(Buffer);

                Packet packet = PacketManager.Instance.DeserializePacket(cSPacketHeader, Buffer, Constant.PacketConstant.PacketHeaderLength);

                StartIndex -= cSPacketHeader.PacketLength + Constant.PacketConstant.PacketHeaderLength;
                Array.Copy(Buffer, cSPacketHeader.PacketLength + Constant.PacketConstant.PacketHeaderLength, Buffer, 0, StartIndex);
                ReferencePool.Release(cSPacketHeader);
                readbufferCallback?.Invoke(this, packet);
            } while (StartIndex > Constant.PacketConstant.PacketHeaderLength);
        }

        public void SendMessage(Socket clientSocket)
        {
            if (_memoryStream.Position == 0)
            {
                return;
            }
            clientSocket.BeginSend(_memoryStream.GetBuffer(), 0, (int)_memoryStream.Length, SocketFlags.None, MessageCallback, null);
        }

        public bool SerilizePacket(Packet packet)
        {
            if (packet == null)
            {
                Log.Error("Error,packet is invalid!");
                return false;
            }
            if (!PacketManager.Instance.Serialize(packet, _memoryStream))
            {
                Log.Error("Error,{0} Serilize error!!", packet.GetType());
                return false;
            }
            else
            {
                return true;
            }
        }

        private void MessageCallback(IAsyncResult ar)
        {
            ResetStream();
        }
        public static Message Create()
        {
            Message message = ReferencePool.Acquire<Message>();
            return message;
        }
        public void ResetStream()
        {
            _memoryStream.Position = 0;
            _memoryStream.SetLength(0);
        }
        public void Clear()
        {
            StartIndex = 0;
            ResetStream();
        }
    }
}