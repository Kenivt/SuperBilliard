namespace SuperBilliardServer.Network.Packets
{
    public class SCPacketHeader : PacketHeaderBase
    {
        public byte flag;
        public override PacketType PacketType => PacketType.ServerToClient;
    }
}