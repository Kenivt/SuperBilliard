using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class LocalizeText : MonoBehaviour
    {
        public string key;

        private void Awake()
        {
            var text = GetComponent<UnityEngine.UI.Text>();
            if (text != null)
            {
                text.text = GameEntry.Localization.GetString(key);
            }
            else
            {
                Log.Error("Can't find Text component.");
            }
        }
    }
}