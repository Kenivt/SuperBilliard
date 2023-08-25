using Knivt.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class SelectToggle : Button
    {
        private Image _selectingTag;

        private bool _isSelecting;

        public bool IsSelecting
        {
            get => _isSelecting;
            set
            {
                _isSelecting = value;
                if (_selectingTag == null)
                {
                    _selectingTag = transform.GetComponentFromOffspring<Image>("SelectingTag");
                }
                _selectingTag.gameObject.SetActive(value);
                if (BindingCanvasGroup != null)
                {
                    if (value == false)
                    {
                        BindingCanvasGroup.alpha = 0;
                        BindingCanvasGroup.blocksRaycasts = false;
                    }
                    else
                    {
                        BindingCanvasGroup.alpha = 1;
                        BindingCanvasGroup.blocksRaycasts = true;
                    }
                }
            }
        }

        public CanvasGroup BindingCanvasGroup;
    }
}