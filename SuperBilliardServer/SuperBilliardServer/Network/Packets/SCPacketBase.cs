namespace SuperBilliardServer.Network.Packets
{
    public abstract class SCPacketBase : PacketBase
    {
        public byte Flag;
        public override PacketType PacketType
        {
            get
            {
                return PacketType.ServerToClient;
            }
        }
    }
}