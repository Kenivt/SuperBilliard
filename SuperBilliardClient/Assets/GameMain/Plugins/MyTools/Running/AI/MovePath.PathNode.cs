using UnityEngine;

namespace Knivt.Tools.AI
{
    [System.Serializable]
    public class TargetPositionNode
    {
        public Vector3 PathNodePosition;
        public float Delay;
    }
    public partial class MovePath : MonoBehaviour
    {
        public enum CycleOptions
        {
            BackAndForth,
            Loop,
            OnlyOnce
        }
    }
}