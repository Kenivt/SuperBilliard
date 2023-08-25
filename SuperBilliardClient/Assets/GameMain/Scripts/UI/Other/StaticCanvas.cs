using UnityEngine;

namespace SuperBilliard
{
    public class StaticCanvas : MonoBehaviour
    {
        private void Awake()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            canvas.sortingOrder = -100;
        }
    }
}
