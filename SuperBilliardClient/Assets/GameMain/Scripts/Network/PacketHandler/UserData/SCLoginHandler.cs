using GameMessage;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCLoginHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.Login;

        public override void Handle(object sender, Packet packet)
        {
            SCLogin sCLogin = (SCLogin)packet;
            if (sCLogin.Result == ReturnResult.Success)
            {
                SuperBilliard.GameEntry.Event.Fire(this, LoginSuccessEventArgs.Create(sCLogin.Username));
            }
            else
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create("登陆失败，请检查账号或密码是否正确..."));
            }
        }
    }
}