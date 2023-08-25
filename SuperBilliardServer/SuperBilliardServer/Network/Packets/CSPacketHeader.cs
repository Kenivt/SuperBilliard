namespace SuperBilliardServer.Network.Packets
{
    public class CSPacketHeader : PacketHeaderBase
    {
        public byte flag;
        public override PacketType PacketType => PacketType.ClientToServer;
    }
}