using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class CSSavePlayerMessage : CSPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSSavePlayerMessage;

        public override void Clear()
        {
            HairId = 1;
            FaceId = 1;
            KitId = 1;
            BodyId = 1;
            UserName = "Default";
            //昵称NickName,拼错了
            SnikName = "Default";
            Description = "Default";
        }

    }
}
