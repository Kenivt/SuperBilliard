using SuperBilliardServer.Network.Packets;


namespace GameMessage
{
    public partial class CSExitGameRoom : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSExitGameRoom;
        public override void Clear()
        {

        }
    }
}
