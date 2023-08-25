using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSGetPlayerMessage : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSGetPlayerMessage;

        public override void Clear()
        {
            gpmUsername_ = "Default";
        }
    }
}
