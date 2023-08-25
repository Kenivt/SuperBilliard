using GameMessage;
using GameFramework;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCGameResultHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.GameResult;

        public override void Handle(object sender, Packet packet)
        {
            SCGameResultPack sCGameResultPack = (SCGameResultPack)packet;
            GameEntry.Event.Fire(this, GameOverEventArgs.Create(sCGameResultPack.Result));
        }
    }
}