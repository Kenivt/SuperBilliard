using Knivt.Tools;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;

namespace SuperBilliard
{
    public class BattleTipUIForm : UILogicBase
    {
        public Image _billiardIdTip;
        private Camera uiCamera;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _billiardIdTip = transform.GetComponentFromOffspring<Image>("BilliardIdTip");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            eventSubscriber.Subscribe(ShowBilliardIdTipEventArgs.EventId, ShowBIlliardIdTipCallBack);
            eventSubscriber.Subscribe(HideBilliardIdTipEventArgs.EventId, HideBilliardIdTipCallBack);
        }

        private void HideBilliardIdTipCallBack(object sender, GameEventArgs e)
        {
            if (_billiardIdTip.gameObject.activeSelf == false)
            {
                return;
            }
            _billiardIdTip.gameObject.SetActive(false);
        }

        private void ShowBIlliardIdTipCallBack(object sender, GameEventArgs e)
        {
            ShowBilliardIdTipEventArgs args = (ShowBilliardIdTipEventArgs)e;
            _billiardIdTip.gameObject.SetActive(true);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, Input.mousePosition, uiCamera, out Vector2 worldPoint))
            {
                _billiardIdTip.rectTransform.anchoredPosition = worldPoint;
            }

            //得到对应的Sprite
            _billiardIdTip.sprite = GameEntry.ResourceCache.GetBIlliardSprite(args.BilliardData);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            eventSubscriber.UnSubscribeAll();
        }
    }
}