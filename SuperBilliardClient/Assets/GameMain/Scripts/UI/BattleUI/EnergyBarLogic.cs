using Knivt.Tools;
using UnityEngine.UI;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class EnergyBarLogic : UILogicBase
    {
        private Image _maskImage;
        private Text _text;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _maskImage = transform.GetComponentFromOffspring<Image>("StorageMask");
            _text = transform.GetComponentFromOffspring<Text>("Text");
            if (_maskImage == null)
                Log.Error("错误,没有找到对应的Image.");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            eventSubscriber.Subscribe(UpdataStorageBarEventArgs.EventId, UpdataStorageBar);
        }

        private void UpdataStorageBar(object sender, GameEventArgs e)
        {
            UpdataStorageBarEventArgs ne = (UpdataStorageBarEventArgs)e;
            if (ne == null)
                Log.Error("错误,UpdataStorageBarEventArgs为空.");
            _maskImage.fillAmount = 1f - ne.FillAmount;
            _text.text = (ne.FillAmount * 100).ToString("F0") + "%";
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            eventSubscriber.UnSubscribeAll();
        }
    }
}