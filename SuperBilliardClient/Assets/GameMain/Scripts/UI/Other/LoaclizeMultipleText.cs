using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    [System.Serializable]
    public struct LocalizationPair
    {
        public string key;
        public string defaultValue;
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class LoaclizeMultipleText : MonoBehaviour
    {
        [Tooltip("第一个值即为默认显示文字")]
        public LocalizationPair[] keyAndValue;
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            if (keyAndValue != null && keyAndValue.Length > 0)
            {
                //获取默认的值
                _text.text = GameEntry.Localization.GetString(keyAndValue[0].key);
            }
        }

        public void DisplayText(int index)
        {
            if (index < 0 || index >= keyAndValue.Length)
            {
                Debug.LogError("index is out of range");
                return;
            }
            _text.text = GameEntry.Localization.GetString(keyAndValue[index].key);
        }
    }
}
