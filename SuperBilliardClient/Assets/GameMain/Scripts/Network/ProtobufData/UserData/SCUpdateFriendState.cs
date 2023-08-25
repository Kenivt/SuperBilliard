using GameMessage;
using GameFramework.Network;
using System.Collections.Generic;

namespace GameMessage
{
    public partial class SCUpdateFriendState : SuperBilliard.SCPacketBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCUpdateFriendState;

        public override void Clear()
        {
            username_ = "defeai";
            status_ = PlayerStatus.PlayerStausNone;
        }
    }
}