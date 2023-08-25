using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCGameStartHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCGameStart;

        public override void Handle(object sender, Packet packet)
        {
            SCGameStart packetImpl = (SCGameStart)packet;

            //开始游戏,延迟两秒开始
            GameEntry.Event.Fire(this, GameStartEventArgs.Create(2f));
        }
    }
}