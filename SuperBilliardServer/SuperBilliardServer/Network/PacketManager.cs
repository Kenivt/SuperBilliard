using ServerCore;
using GameFramework;
using System.Reflection;
using ServerCore.Sington;
using SuperBilliardServer.Tools;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Constant;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.PacketHandlers;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network
{
    public class PacketManager : SingtonBase<PacketManager>
    {
        private readonly Dictionary<int, Type> _serverToClientPacketTypes = new Dictionary<int, Type>();

        private readonly Dictionary<int, IPacketHandler> _packetHandlerDic = new Dictionary<int, IPacketHandler>();

        public PacketManager()
        {
            Initlize();
        }

        private void Initlize()
        {
            Type packetBaseType = typeof(CSPacketBase);
            //Type packhandlerBaseType = typeof(PacketHandlerBase);
            Type packetAsyncHandler = typeof(PacketAsyncHandler);
            Type packetSyncHandler = typeof(PacketSyncHandler);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsClass || types[i].IsAbstract)
                {
                    continue;
                }

                if (types[i].BaseType == packetBaseType)
                {
                    PacketBase packetBase = (PacketBase)Activator.CreateInstance(types[i]);
                    Type packetType = GetServerToClientPacketType(packetBase.Id);
                    if (packetType != null)
                    {
                        Console.WriteLine("Already exist packet type '{0}', check '{1}' or '{2}'?.", packetBase.Id.ToString(), packetType.Name, packetBase.GetType().Name);
                        continue;
                    }

                    _serverToClientPacketTypes.Add(packetBase.Id, types[i]);
                }
                else if (types[i].BaseType == packetAsyncHandler || types[i].BaseType == packetSyncHandler)
                {
                    IPacketHandler packetHandler = (IPacketHandler)Activator.CreateInstance(types[i]);
                    _packetHandlerDic.Add(packetHandler.Id, packetHandler);
                }
            }
        }

        public Type GetServerToClientPacketType(int id)
        {
            if (_serverToClientPacketTypes.TryGetValue(id, out Type type))
            {
                return type;
            }
            return null;
        }

        public IPacketHandler GetPacketHandler(int id)
        {
            if (_packetHandlerDic.TryGetValue(id, out IPacketHandler packetHandler))
            {
                return packetHandler;
            }
            return null;
        }

        public void ExecutePackHandle(object sender, Client client, Packet packet)
        {
            IPacketHandler packetHandler = GetPacketHandler(packet.Id);
            if (packetHandler == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"错误,没有找到对应ID:{packet.Id}的Handler.");
                Console.ResetColor();
            }
            //将执行操作放到主线程中执行,防止多线程操作引起的问题
            MainThreadSyncContext.Instance.Post(() =>
            {
                packetHandler?.Handle(sender, client, packet);
            });
        }

        // -----------------------------------序列化部分-------------------------------

        private readonly List<byte[]> _bytesList = new List<byte[]>() { new byte[1], new byte[2], new byte[2] };

        public int PacketHeaderLength => PacketConstant.PacketHeaderLength;

        public bool Serialize<T>(T packet, Stream destination) where T : Packet
        {
            using (MemoryStream stream = new MemoryStream())
            {
                SCPacketBase packetImpl = packet as SCPacketBase;
                if (packetImpl == null)
                {
                    Log.Warning("Packet is invalid.");
                    return false;
                }

                if (packetImpl.PacketType != PacketType.ServerToClient)
                {
                    Log.Warning("Send packet invalid.");
                    return false;
                }

                stream.Seek(PacketHeaderLength, SeekOrigin.Begin);
                stream.SetLength(PacketHeaderLength);
                ProtobufHelper.ToStream(packet, stream);

                CSPacketHeader packetHeader = ReferencePool.Acquire<CSPacketHeader>();
                packetHeader.flag = 0;
                packetHeader.Id = (ushort)packet.Id;
                packetHeader.PacketLength = (int)stream.Length - PacketHeaderLength;

                lock (_bytesList)
                {
                    _bytesList[1][0] = packetHeader.flag;
                    _bytesList[1].WriteTo(0, packetHeader.Id);
                    _bytesList[2].WriteTo(0, (ushort)packetHeader.PacketLength);
                    //写入头部数据
                    int index = 0;
                    foreach (var bytes in _bytesList)
                    {
                        Array.Copy(bytes, 0, stream.GetBuffer(), index, bytes.Length);
                        index += bytes.Length;
                    }
                }
                ReferencePool.Release(packetHeader);
                //ReferencePool.Release(packet);
                //写入序列化的流数据
                stream.WriteTo(destination);
            }
            return true;
        }

        public Packet DeserializePacket(PacketHeaderBase packetHeader, byte[] buffer, int startIndex)
        {
            CSPacketHeader header = packetHeader as CSPacketHeader;
            if (header == null)
            {
                throw new Exception("Packet header is invalid.");
            }
            Packet packet = null;
            if (header.IsValid)
            {
                Type packetType = Instance.GetServerToClientPacketType(header.Id);

                if (packetType != null && buffer != null)
                {
                    object instance = ReferencePool.Acquire(packetType);
                    packet = (Packet)ProtobufHelper.FromBytes(instance, buffer, startIndex, header.PacketLength);
                }
                else
                {
                    throw new Exception($"Can not deserialize packet for packet id '{header.Id}'.");
                }
            }
            else
            {
                throw new Exception("Packet header is invalid.");
            }
            return packet;
        }

        public PacketHeaderBase DeserializePacketHeader(byte[] buffer)
        {
            CSPacketHeader csPacketHeader = ReferencePool.Acquire<CSPacketHeader>();
            csPacketHeader.flag = buffer[0];
            csPacketHeader.Id = BitConverter.ToUInt16(buffer, PacketConstant.PacketIdIndex);
            csPacketHeader.PacketLength = BitConverter.ToUInt16(buffer, PacketConstant.PacketLengthIndex);

            return csPacketHeader;
        }

    }
}