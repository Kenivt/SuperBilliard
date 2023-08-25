using GameMessage;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class LoginUIForm : UILogicBase
    {
        [Header("=========CanvasGroup==========")]
        [SerializeField] private CanvasGroup loginCanvasGroup;
        [SerializeField] private CanvasGroup registerCanvasGroup;
        [Header("=========Button==========")]
        [SerializeField] private Button switchRigisterBtn;
        [SerializeField] private Button switchLoginBtn;
        [SerializeField] private Button loginBtn;
        [SerializeField] private Button registerBtn;
        [Header("=========InputField==========")]
        [SerializeField] private InputField loginUsernameInput;
        [SerializeField] private InputField loginPasswordInput;
        [SerializeField] private InputField registerUsernameInput;
        [SerializeField] private InputField registerPasswordInput;
        [SerializeField] private InputField registerPasswordConfirmInput;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Listen();
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            Unlisten();
        }
        private void Listen()
        {
            switchLoginBtn.onClick.AddListener(() =>
            {
                loginCanvasGroup.alpha = 1;
                loginCanvasGroup.blocksRaycasts = true;
                registerCanvasGroup.alpha = 0;
                registerCanvasGroup.blocksRaycasts = false;
            });
            switchRigisterBtn.onClick.AddListener(() =>
            {
                loginCanvasGroup.alpha = 0;
                loginCanvasGroup.blocksRaycasts = false;
                registerCanvasGroup.alpha = 1;
                registerCanvasGroup.blocksRaycasts = true;
            });
            loginBtn.onClick.AddListener(() =>
            {
                string username = loginUsernameInput.text;
                string password = loginPasswordInput.text;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.PleaseEnterPassword)));
                    return;
                }
                if (username.Length < 12)
                {
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.UserNameLenghtLimit)));
                    return;
                }
                CSLogin csLogin = new CSLogin();
                csLogin.Username = username;
                csLogin.Password = password;
                GameEntry.Client.Send(csLogin);
            });
            registerBtn.onClick.AddListener(() =>
            {
                string username = registerUsernameInput.text;
                string password = registerPasswordInput.text;
                string passwordConfirm = registerPasswordConfirmInput.text;
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirm))
                {
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.PleaseEnterPassword)));
                    return;
                }

                if (password != passwordConfirm)
                {
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.ConfirmPasswordErrored)));
                    return;
                }
                if (username.Length < 12)
                {
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(LoginPrompt.UserNameLenghtLimit)));
                    return;
                }
                CSRigister csRigister = new CSRigister();
                csRigister.Username = username;
                csRigister.Password = password;
                GameEntry.Client.Send(csRigister);
            });
        }
        private void Unlisten()
        {
            switchLoginBtn.onClick.RemoveAllListeners();
            switchRigisterBtn.onClick.RemoveAllListeners();
            loginBtn.onClick.RemoveAllListeners();
            registerBtn.onClick.RemoveAllListeners();
        }
    }
}