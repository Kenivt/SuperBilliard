namespace SuperBilliardServer.Network.Packets
{
    public abstract class CSPacketBase : PacketBase
    {
        public byte Flag;
        public override PacketType PacketType
        {
            get
            {
                return PacketType.ClientToServer;
            }
        }
    }
}