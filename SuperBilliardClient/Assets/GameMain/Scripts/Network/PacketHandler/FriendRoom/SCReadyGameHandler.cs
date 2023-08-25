using GameMessage;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SCReadyGameHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCReadyGame;

        public override void Handle(object sender, Packet packet)
        {
            var sCReadyGame = (SCReadyGame)packet;

            FriendRoomDataBundle dataBundle = GameEntry.DataBundle.GetData<FriendRoomDataBundle>();
            //设置对应的数据
            Log.Warning("收到玩家准备状态更新消息" + sCReadyGame.Username + " | " + sCReadyGame.IsReady);
            dataBundle.SetReadyState(sCReadyGame.Username, sCReadyGame.IsReady);
        }
    }
}