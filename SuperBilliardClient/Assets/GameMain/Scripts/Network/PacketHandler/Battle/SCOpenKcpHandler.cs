using System.Net;
using GameMessage;
using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SCOpenKcpHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCOpenKcp;

        public override void Handle(object sender, Packet packet)
        {
            SCOpenKcp sCGameStartPack = (SCOpenKcp)packet;
            string key = sCGameStartPack.KcpIpEnd;
            string[] ipEnd = key.Split('|');

            if (ipEnd.Length != 2)
            {
                Log.Error("ipEnd.Length != 2"); return;
            }

            int port = int.Parse(ipEnd[1]);
            IPAddress ip = IPAddress.Parse(ipEnd[0]);
            GameEntry.Client.KcpEndPoint = new IPEndPoint(ip, port);
        }
    }
}