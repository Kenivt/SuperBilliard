using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public interface IMessageTip
    {
        CanvasGroup CanvasGroup { get; }
        Transform Transform { get; }
        void ShowMessage(string message);
        void SetActive(bool active);
    }
    public class NormalMessageTip : MonoBehaviour, IMessageTip
    {
        public CanvasGroup CanvasGroup => _canvasGroup;
        public Transform Transform => transform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Text _text;
        public void ShowMessage(string message)
        {
            _text.text = message;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
