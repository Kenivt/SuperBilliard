using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class Message : MonoBehaviour
    {
        public float moveTime;
        private float _timer;

        public CanvasGroup CanvasGroup => _canvasGroup;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Text _messageText;

        private string message;
        private RectTransform _rectTransform;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        /// <summary>
        /// 是否在正常运行Tween
        /// </summary>
        public bool Tween(float elaspTime, out float processing)
        {
            _timer += elaspTime;
            processing = Mathf.Clamp01(_timer / moveTime);
            if (_timer > moveTime)
            {
                return false;
            }
            return true;
        }
        public void ResetMessage()
        {
            _timer = 0;
        }

        public void SetMessage(string message)
        {
            _messageText.text = message;
        }
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        public Vector2 localPos
        {
            get
            {
                return _rectTransform.anchoredPosition;
            }
            set
            {
                _rectTransform.anchoredPosition = value;
            }
        }
    }
}
