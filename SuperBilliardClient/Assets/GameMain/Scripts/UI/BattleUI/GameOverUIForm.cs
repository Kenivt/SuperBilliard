using GameMessage;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class GameOverUIForm : UILogicBase
    {
        public Text resultText;

        public Button remarchBtn;
        public Button backMainMenuBtn;

        public Color victoryColor;
        public Color defeatColor;

        private Image _bcg;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _bcg = GetComponent<Image>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameResult result = (GameResult)userData;
            if (result == GameResult.Victory)
            {
                _bcg.color = victoryColor;
                resultText.text = "VICTORY";
            }
            else
            {
                _bcg.color = defeatColor;
                resultText.text = "DEFEAT";
            }
            remarchBtn.onClick.AddListener(OnRemarchBtnClick);
            backMainMenuBtn.onClick.AddListener(OnBackMainMenuBtnClick);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            remarchBtn.onClick.RemoveListener(OnRemarchBtnClick);
            backMainMenuBtn.onClick.RemoveListener(OnBackMainMenuBtnClick);
        }

        private void OnBackMainMenuBtnClick()
        {
            GameEntry.Client.SendExitGameRoom();
            GameEntry.Event.Fire(this, BackMainMenuEventArgs.Create());
            //关闭自己
            Close();
        }

        private void OnRemarchBtnClick()
        {
            //GameEntry.Event.Fire(this, BackMainMenuEventArgs.Create());
        }
    }
}