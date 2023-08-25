using GameFramework;
using SuperBilliardServer.Network.Packets;

namespace GameMessage
{
    public partial class SCStartTurn : SCPacketBase
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.SCStartTurn;

        public override void Clear()
        {
            isPlacewhite_ = false;
        }
        public static SCStartTurn Create(bool isPlacewhite)
        {
            SCStartTurn sCStartTurn = ReferencePool.Acquire<SCStartTurn>();
            sCStartTurn.isPlacewhite_ = isPlacewhite;
            return sCStartTurn;
        }
    }
}
