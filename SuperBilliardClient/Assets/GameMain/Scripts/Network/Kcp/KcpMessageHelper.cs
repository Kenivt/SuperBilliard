using System;
using System.IO;
using GameFramework;
using System.Reflection;
using GameFramework.Network;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class KcpMessageHelper
    {
        private readonly Dictionary<int, Type> m_ServerToClientPacketTypes = new Dictionary<int, Type>();
        private readonly MemoryStream m_CachedStream = new MemoryStream(1024 * 8);
        private readonly List<byte[]> byteses = new List<byte[]>() { new byte[1], new byte[2], new byte[2] };
        private readonly Dictionary<int, IPacketHandler> _packetHandlerDic = new Dictionary<int, IPacketHandler>();

        public int PacketHeaderLength => Constant.PacketConstant.PacketHeaderLength;
        public KcpMessageHelper()
        {
            Initialize();
        }
        public void Initialize()
        {
            // 反射注册包和包处理函数
            Type packetBaseType = typeof(SCPacketBase);
            Type packetHandlerBaseType = typeof(PacketHandlerBase);
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
                        Log.Warning("Already exist packet type '{0}', check '{1}' or '{2}'?.", packetBase.Id.ToString(), packetType.Name, packetBase.GetType().Name);
                        continue;
                    }

                    m_ServerToClientPacketTypes.Add(packetBase.Id, types[i]);
                }
                else if (types[i].BaseType == packetHandlerBaseType)
                {
                    IPacketHandler packetHandler = (IPacketHandler)Activator.CreateInstance(types[i]);
                    _packetHandlerDic.Add(packetHandler.Id, packetHandler);
                }
            }
        }
        public bool GetPacketHandler(int packetId, out IPacketHandler packetHandler)
        {
            if (_packetHandlerDic.TryGetValue(packetId, out packetHandler) == false)
            {
                Log.Error("Not found packet handler: " + packetId);
                return false;
            }
            return true;
        }
        public void Shutdown()
        {

        }
        /// <summary>
        /// 序列化消息包。
        /// </summary>
        /// <typeparam name="T">消息包类型。</typeparam>
        /// <param name="packet">要序列化的消息包。</param>
        /// <param name="destination">要序列化的目标流。</param>
        /// <returns>是否序列化成功。</returns>
        public bool Serialize<T>(T packet, Stream destination) where T : Packet
        {
            PacketBase packetImpl = packet as PacketBase;
            if (packetImpl == null)
            {
                Log.Warning("Packet is invalid.");
                return false;
            }

            if (packetImpl.PacketType != PacketType.ClientToServer)
            {
                Log.Warning("Send packet invalid.");
                return false;
            }

            m_CachedStream.Seek(PacketHeaderLength, SeekOrigin.Begin);
            m_CachedStream.SetLength(PacketHeaderLength);
            ProtobufHelper.ToStream(packet, m_CachedStream);

            // 头部消息
            CSPacketHeader packetHeader = ReferencePool.Acquire<CSPacketHeader>();
            packetHeader.flag = 0;
            packetHeader.Id = (ushort)packet.Id;
            packetHeader.PacketLength = (int)m_CachedStream.Length - PacketHeaderLength;

            m_CachedStream.Position = 0;
            //塞入头部的信息
            this.byteses[0][0] = packetHeader.flag;
            this.byteses[1].WriteTo(0, (ushort)packetHeader.Id);
            this.byteses[2].WriteTo(0, (ushort)packetHeader.PacketLength);
            int index = 0;
            foreach (var bytes in this.byteses)
            {
                Array.Copy(bytes, 0, m_CachedStream.GetBuffer(), index, bytes.Length);
                index += bytes.Length;
            }
            ReferencePool.Release(packetHeader);
            ReferencePool.Release(packet);
            m_CachedStream.WriteTo(destination);
            return true;
        }

        public PacketHeaderBase DeserializePacketHeader(byte[] buffer, out object customErrorData)
        {
            customErrorData = null;

            CSPacketHeader csPacketHeader = ReferencePool.Acquire<CSPacketHeader>();
            csPacketHeader.flag = buffer[0];
            csPacketHeader.Id = BitConverter.ToUInt16(buffer, Constant.PacketConstant.PacketIdIndex);
            csPacketHeader.PacketLength = BitConverter.ToUInt16(buffer, Constant.PacketConstant.PacketLengthIndex);

            return csPacketHeader;
        }

        public Packet DeserializePacket(PacketHeaderBase packetHeader, byte[] buffer, int startIndex, out object customErrorData)
        {
            customErrorData = null;
            CSPacketHeader header = packetHeader as CSPacketHeader;
            if (header == null)
            {
                throw new Exception("Packet header is invalid.");
            }
            Packet packet = null;
            if (header.IsValid)
            {
                Type packetType = GetServerToClientPacketType(header.Id);

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

        private Type GetServerToClientPacketType(int id)
        {
            Type type = null;
            if (m_ServerToClientPacketTypes.TryGetValue(id, out type))
            {
                return type;
            }

            return null;
        }
    }
}