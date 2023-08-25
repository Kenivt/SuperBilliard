using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCUpdateFriendStateHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCUpdateFriendState;

        public override void Handle(object sender, Packet packet)
        {
            SCUpdateFriendState scPacket = (SCUpdateFriendState)packet;

            FriendDataBundle friendDataBundle = GameEntry.DataBundle.GetData<FriendDataBundle>();
            friendDataBundle.UpdateFriendState(scPacket.Username, scPacket.Status);
            GameEntry.Event.Fire(this, UpdateFriendStateEventArgs.Create(scPacket.Username, scPacket.Status));
        }
    }
}