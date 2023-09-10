using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public abstract class PacketSyncHandler : PacketHandlerBase
    {
        public override bool IsAsync => false;
        public sealed override Task HandleAsync(object sender, Client client, Packet packet)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class PacketAsyncHandler : PacketHandlerBase
    {
        public override bool IsAsync => true;
        public sealed override void HandleSync(object sender, Client client, Packet packet)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract int Id
        {
            get;
        }
        public abstract bool IsAsync
        {
            get;
        }
        public async void Handle(object sender, Client client, Packet packet)
        {
            try
            {
                if (IsAsync)
                {
                    await HandleAsync(sender, client, packet);
                }
                else
                {
                    HandleSync(sender, client, packet);
                }
                ReferencePool.Release(packet);
            }
            catch (Exception e)
            {
                Log.Error("=======PACKET HANDLE ERROR=======PacketHandler :{0},\nErrorMessage :{1}",
                    this.GetType().ToString(), e.Message);
            }
        }
        public abstract Task HandleAsync(object sender, Client client, Packet packet);
        public abstract void HandleSync(object sender, Client client, Packet packet);
    }
}
