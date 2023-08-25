using UnityEngine;
using System.Collections.Generic;

namespace Knivt.Tools.AI
{
    public partial class MovePath : MonoBehaviour
    {
        public CycleOptions cycleOptions;

        public List<TargetPositionNode> MovePathNodeList = new List<TargetPositionNode>();

        public bool LockHandlesOnXAxis;
        public bool LockHandlesOnYAxis;
        public bool LockHandlesOnZAxis;

        public int Count => MovePathNodeList.Count;
        public Vector3? GetPos(int index)
        {
            if (index < 0 && index >= MovePathNodeList.Count)
            {
                Debug.LogError("错误,索引越界");
                return null;
            }

            return MovePathNodeList[index].PathNodePosition + OriginalTransformPosition;
        }
        public float? GetDelay(int index)
        {
            if (index < 0 && index >= MovePathNodeList.Count)
            {
                Debug.LogError("错误,索引越界");
                return null;
            }

            return MovePathNodeList[index].Delay;
        }


        public Vector3 OriginalTransformPosition => _orignalTransformPosition;

        [SerializeField] private Vector3 _orignalTransformPosition;

        /// <summary>
        /// 是否需要初始化OrignalPosition
        /// </summary>
        public bool NeedResetOrignalPosition { get; set; } = false;
        private bool _active = false;

        private void Awake()
        {
            _active = true;
            _nodeReader = new TargetPostionNodeReader(MovePathNodeList, cycleOptions);
        }
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (MovePathNodeList == null || MovePathNodeList.Count == 0)
            {
                return;
            }

            //重定原点
            if (NeedResetOrignalPosition)
            {
                _orignalTransformPosition = this.transform.position;
                NeedResetOrignalPosition = false;
            }
            if (transform.hasChanged && !_active)
            {
                _orignalTransformPosition = this.transform.position;
            }

            //DrawGizmo
            for (int i = 0; i < MovePathNodeList.Count; i++)
            {
                DrawGizmoPoint(OriginalTransformPosition + MovePathNodeList[i].PathNodePosition, 0.2f, Color.green);
                if ((i + 1) < MovePathNodeList.Count)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(_orignalTransformPosition + MovePathNodeList[i].PathNodePosition,
                        _orignalTransformPosition + MovePathNodeList[i + 1].PathNodePosition);
                }
                if (i == MovePathNodeList.Count - 1 && cycleOptions == CycleOptions.Loop)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(_orignalTransformPosition + MovePathNodeList[i].PathNodePosition,
                        _orignalTransformPosition + MovePathNodeList[0].PathNodePosition);
                }
            }
#endif
        }
        private void DrawGizmoPoint(Vector3 position, float size, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(position, size);
        }
    }
}