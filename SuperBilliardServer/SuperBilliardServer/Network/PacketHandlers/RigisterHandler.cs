using GameMessage;
using GameFramework;
using SuperBilliardServer.Sql;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.PacketHandlers
{
    public class RigisterHandler : PacketAsyncHandler
    {
        public override int Id => Constant.PacketTypeId.Rigister;

        public override async Task HandleAsync(object sender, Client client, Packet packet)
        {
            CSRigister rigister = packet as CSRigister;
            Console.WriteLine("有注册行为!!!:昵称:" + rigister.Username + "密码:" + rigister.Password);

            bool rigisterResult = await SqlManager.Instance.GetSqlHandler<ILoginSqlHandler>().Rigister(rigister.Username, rigister.Password);
            SCRigister sCRigister = ReferencePool.Acquire<SCRigister>();
            Log.Debug(rigisterResult);
            if (rigisterResult)
            {
                sCRigister.Result = ReturnResult.Success;
            }
            else
            {
                sCRigister.Result = ReturnResult.Failure;
            }
            client.SerilizePacketToMessages(sCRigister);
            client.Send();
            //释放资源
            ReferencePool.Release(sCRigister);
        }
    }
}