using GameFramework;

namespace SuperBilliardServer.Network.Packets
{
    public abstract class Packet : IReference
    {
        public abstract int Id
        {
            get;
        }
        public abstract void Clear();
    }
}