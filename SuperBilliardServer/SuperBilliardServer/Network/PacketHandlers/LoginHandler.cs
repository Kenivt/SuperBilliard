using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.PacketHandlers;

namespace GameMessage
{
    public class LoginHandler : PacketAsyncHandler
    {
        public override int Id => SuperBilliardServer.Constant.PacketTypeId.Login;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            CSLogin cSLogin = packet as CSLogin;
            if (cSLogin == null)
            {
                Log.Error("CSLogin为空");
                return;
            }

            SCLogin sCLogin = ReferencePool.Acquire<SCLogin>();
            string username = cSLogin.Username;
            string password = cSLogin.Password;

            await SqlManager.Instance.GetSqlHandler<ILoginSqlHandler>().Login(username, password, sCLogin);
            if (sCLogin.Result == ReturnResult.Success)
            {
                PlayerManager.Instance.RigisterPlayer(username, client);
            }
            client.SendPacket(sCLogin);
            //释放资源
            ReferencePool.Release(sCLogin);
        }
    }
}
