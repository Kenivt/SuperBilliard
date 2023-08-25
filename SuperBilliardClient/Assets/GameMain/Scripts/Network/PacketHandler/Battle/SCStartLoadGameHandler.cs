using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    //开始加载游戏
    public class SCStartLoadGameHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCStartLoadGame;

        public override void Handle(object sender, Packet packet)
        {
            SCStartLoadGame sCGameStartPack = (SCStartLoadGame)packet;

            EnumBattle dataBundle = sCGameStartPack.GameType.ToEnumBattle();

            //发送开始信号

            GameEntry.Event.Fire(this, StartLoadGameEventArgs.Create(dataBundle,
                sCGameStartPack.OpponentName, sCGameStartPack.IsFirstMove, sCGameStartPack.RandomSeed));
        }
    }
}