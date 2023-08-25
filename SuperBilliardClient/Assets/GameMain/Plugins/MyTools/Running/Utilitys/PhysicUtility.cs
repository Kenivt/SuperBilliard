using UnityEngine;
using System.Collections.Generic;

namespace Knivt.Tools
{
    public static class PhysicUtility
    {
        public static Collider[] CheckViewRangeSphere(Vector3 center, float radius, Vector3 forword, float angle, LayerMask layerMask)
        {
            List<Collider> list = new List<Collider>();
            CheckViewRangeSphere(center, radius, forword, angle, layerMask, list);
            return list.ToArray();
        }
        /// <summary>
        /// 目标位置,是否在前方指定角度区域内
        /// </summary>
        public static bool OnAngleRange(Vector3 targetPos, Vector3 center, float radius, Vector3 forword, float angle)
        {
            float angleHalf = angle / 2;
            targetPos.y = center.y;
            Vector3 dir = (targetPos - center).normalized;
            float theAngle = Vector3.Angle(dir, forword);
            return theAngle < angleHalf;
        }
        public static Collider[] CheckViewRangeSphere(Vector3 center, float radius, Vector3 forword, float angle, LayerMask layerMask, List<Collider> list)
        {
            Collider[] colliders = Physics.OverlapSphere(center, radius, layerMask);
            float angleHalf = angle / 2;
            foreach (var item in colliders)
            {
                Vector3 pos = item.transform.position;
                pos.y = center.y;
                Vector3 dir = (pos - center).normalized;
                float theAngle = Vector3.Angle(dir, forword);
                if (theAngle < angleHalf)
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 此函数会检查三个点是否全部都被遮挡,如果有一个点没被遮挡则返回false
        /// </summary>
        public static bool CheckBarrier(Vector3 position, Vector3 targetPosition, float targetWidth, float distance, LayerMask barrierMask, bool isDebug = false)
        {
            Vector3 diraction = new Vector3(targetPosition.x - position.x, 0, targetPosition.z - position.z);

            //获得垂直向量
            Vector3 verticalVector = Vector3.Cross(diraction, Vector3.up).normalized;

            //获得两侧的点
            float halfWidth = targetWidth / 2f;
            Vector3 midPos = targetPosition;
            Vector3 firstPos = verticalVector.normalized * halfWidth + midPos;
            Vector3 secondPos = verticalVector.normalized * -halfWidth + midPos;

            bool flag1 = Physics.Raycast(position, (firstPos - position).normalized, distance, barrierMask);
            bool flag2 = Physics.Raycast(position, (secondPos - position).normalized, distance, barrierMask);
            bool flag3 = Physics.Raycast(position, (midPos - position).normalized, distance, barrierMask);
            if (isDebug)
            {
                Color color = flag1 ? Color.blue : Color.black;
                Debug.DrawLine(position, firstPos, color, 10f);
                color = flag2 ? Color.blue : Color.black;
                Debug.DrawLine(position, secondPos, color, 10f);
                color = flag3 ? Color.blue : Color.black;
                Debug.DrawLine(position, midPos, color, 10f);
                Debug.Log("Debug一次...");
            }
            return flag1 && flag2 && flag3;
        }
    }
}
