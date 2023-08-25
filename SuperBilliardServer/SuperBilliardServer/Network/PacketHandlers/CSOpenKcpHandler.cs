using GameMessage;
using GameFramework;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public partial class CSOpenKcpHandler : PacketSyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.CSOpenKcp;

        public override void HandleSync(object sender, Client client, Packet packet)
        {
            CSOpenKcp cSOpenKcp = (CSOpenKcp)packet;
            //开启KCP
            client.OpenKcp(cSOpenKcp.KcpIpEnd);

            string key = KcpClient.GetKcpIpEndPointKey(client.Kcp.ownIPEndPoint);
            var sc = SCOpenKcp.Create(key);

            client.SendKcpPacket(sc);
            ReferencePool.Release(sc);
        }
    }
}
