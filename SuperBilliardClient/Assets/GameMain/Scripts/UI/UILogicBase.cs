using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public abstract class UILogicBase : UIFormLogic
    {
        public const int DepthFactor = 5;
        private const float FadeTime = 0.3f;

        private static Font s_MainFont = null;
        private Canvas _cachedCanvas = null;
        protected CanvasGroup _canvasGroup = null;
        private CanvasScaler _canvasScaler = null;
        private List<Canvas> _cachedCanvasContainer = new List<Canvas>();
        protected EventSubscriber eventSubscriber { get; private set; }
        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return _cachedCanvas.sortingOrder;
            }
        }
        private bool _hasClosed = false;

        private IEnumerator _closeEnum;
        private IEnumerator _openEnum;
        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            if (_hasClosed == false)
            {
                _hasClosed = true;
                if (ignoreFade)
                {
                    GameEntry.UI.CloseUIForm(this);
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(CloseCo(FadeTime));
                }
            }
        }
        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            _cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            _cachedCanvas.overrideSorting = true;
            OriginalDepth = _cachedCanvas.sortingOrder;
            _cachedCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            GameObject uiCamera = GameObject.FindGameObjectWithTag("UICamera");
            _cachedCanvas.worldCamera = uiCamera.GetComponent<Camera>();
            _canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            _canvasScaler = gameObject.GetOrAddComponent<CanvasScaler>();
            _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            eventSubscriber = EventSubscriber.Create(this);
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _hasClosed = false;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;
            //StopAllCoroutines();
            //StartCoroutine(OpenCo(FadeTime));
        }

        protected override void OnClose(bool isShutdown, object userData)

        {
            base.OnClose(isShutdown, userData);
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnResume()
#else
        protected internal override void OnResume()
#endif
        {
            base.OnResume();
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            //StopAllCoroutines();
            //StartCoroutine(_canvasGroup.FadeToAlpha(1f, FadeTime));
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnCover()
#else
        protected internal override void OnCover()
#endif
        {
            base.OnCover();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnReveal()
#else
        protected internal override void OnReveal()
#endif
        {
            base.OnReveal();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRefocus(object userData)
#else
        protected internal override void OnRefocus(object userData)
#endif
        {
            base.OnRefocus(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#else
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#endif
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UIGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, _cachedCanvasContainer);
            for (int i = 0; i < _cachedCanvasContainer.Count; i++)
            {
                _cachedCanvasContainer[i].sortingOrder += deltaDepth;
            }

            _cachedCanvasContainer.Clear();
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return _canvasGroup.FadeToAlpha(0f, duration);
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            GameEntry.UI.CloseUIForm(this);
        }
        private IEnumerator OpenCo(float duration)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 0;
            yield return _canvasGroup.FadeToAlpha(1f, duration);
            _canvasGroup.alpha = 1;
        }
    }
}