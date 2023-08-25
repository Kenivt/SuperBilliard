using GameFramework.Fsm;
using GameFramework.Event;

namespace SuperBilliard
{
    public class ProcedureLogin : ProcedureBase
    {
        public override bool UseNativeDialog => throw new System.NotImplementedException();

        private bool _isLogin = false;

        protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _isLogin = false;
            //打开主客户端
            GameEntry.Client.OpenMainClient();
            GameEntry.Client.InitKcp();

            GameEntry.UI.OpenUIForm(EnumUIForm.LoginPanel);
            GameEntry.UI.OpenUIForm(EnumUIForm.MessageUIForm);

            GameEntry.Event.Subscribe(LoginSuccessEventArgs.EventId, LoginSucessCallback);
            GameEntry.Event.Subscribe(LoginFailureEventArgs.EventId, LoginFailureCallback);
            GameEntry.Event.Subscribe(RigisterFailureEventArgs.EventId, RigisterFailureCallBack);
            GameEntry.Event.Subscribe(RigisterSuccessEventArgs.EventId, RigisterSuccessCallBack);
        }

        protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (_isLogin)
            {
                ChangeState<ProcedureLoadMessage>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(LoginSuccessEventArgs.EventId, LoginSucessCallback);
            GameEntry.Event.Unsubscribe(LoginFailureEventArgs.EventId, LoginFailureCallback);
            GameEntry.Event.Unsubscribe(RigisterFailureEventArgs.EventId, RigisterFailureCallBack);
            GameEntry.Event.Unsubscribe(RigisterSuccessEventArgs.EventId, RigisterSuccessCallBack);
        }

        private void RigisterFailureCallBack(object sender, GameEventArgs e)
        {
            GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.RegisterFail)));
        }

        private void RigisterSuccessCallBack(object sender, GameEventArgs e)
        {
            GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.RegisterSuccess)));

        }

        private void LoginFailureCallback(object sender, GameEventArgs e)
        {
            GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.WrongPassword)));
        }

        private void LoginSucessCallback(object sender, GameEventArgs e)
        {
            LoginSuccessEventArgs loginSuccessEventArgs = e as LoginSuccessEventArgs;
            _isLogin = true;
            PlayerDataBundle playerDataBunlde = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            playerDataBunlde.UserName = loginSuccessEventArgs.userName;
        }
    }
}