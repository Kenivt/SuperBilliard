using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public interface IPacketHandler
    {
        /// <summary>
        /// 获取网络消息包协议编号。
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// 网络消息包处理函数,此函数会被发送到主线程执行,线程安全可以调用其他主线程资源而不用加锁
        /// </summary>
        /// <param name="sender">网络消息包源。</param>
        /// <param name="packet">网络消息包内容。</param>
        void Handle(object sender, Client client, Packet packet);
    }
}
