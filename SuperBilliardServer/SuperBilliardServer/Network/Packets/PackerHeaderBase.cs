using GameFramework;

namespace SuperBilliardServer.Network.Packets
{
    public abstract class PacketHeaderBase : IReference
    {
        public abstract PacketType PacketType
        {
            get;
        }

        public ushort Id
        {
            get;
            set;
        }

        public int PacketLength
        {
            get;
            set;
        }

        public bool IsValid
        {
            get
            {
                return PacketType != PacketType.Undefined && Id > 0 && PacketLength >= 0;
            }
        }

        public void Clear()
        {
            Id = 0;
            PacketLength = 0;
        }
    }

}