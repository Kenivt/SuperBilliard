using UnityEngine;

namespace SuperBilliard
{
    public class RangeTest : MonoBehaviour
    {
        private void Update()
        {
            if (LevelManager.Instance.OnRange(Camera.main.ScreenToWorldPoint(Input.mousePosition), RangeType.SnokkerHalfCircle))
            {
                Debug.Log("在范围之内........");
            }
            else
            {
                Debug.LogWarning("没有在范围之内........");
            }
        }
    }
}
