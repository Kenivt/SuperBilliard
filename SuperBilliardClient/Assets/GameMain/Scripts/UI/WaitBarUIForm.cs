using GameMessage;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class WaitBarUIForm : UILogicBase
    {
        public Button waitingBtn;
        public Text waitTimeText;

        private float startTimer;

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            startTimer += elapseSeconds;
            waitTimeText.text = TimeUtility.TimeConvert(startTimer);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            startTimer = 0;
            waitingBtn.onClick.AddListener(() =>
            {
                CSStopMatch cSStopMatch = CSStopMatch.Create();
                GameEntry.Client.Send(cSStopMatch);
                Close();
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            waitingBtn.onClick.RemoveAllListeners();
        }
    }
}