using UnityEngine;

namespace Knivt.Tools
{
    public static class GizmoUtility
    {
        public static void DrawArc(Vector3 center, Vector3 from, float angle, float radius, int segments)
        {
            Vector3 formDic = (from - center).normalized;
            angle = angle % 360f;
            for (int i = 0; i < segments; i++)
            {
                float t1 = i / (float)segments;
                float t2 = (i + 1) / (float)segments;

                float angle1 = Mathf.Lerp(0, angle, t1);
                float angle2 = Mathf.Lerp(0, angle, t2);

                Vector3 dir1 = Quaternion.Euler(0, angle1, 0) * formDic;
                Vector3 dir2 = Quaternion.Euler(0, angle2, 0) * formDic;

                Vector3 arcPoint1 = center + dir1 * radius;
                Vector3 arcPoint2 = center + dir2 * radius;

                Gizmos.DrawLine(arcPoint1, arcPoint2);
            }
        }
        public static void DrawViewRange(Vector3 center, Vector3 forword, float angle, float radius)
        {
            float halfAngle = angle / 2;
            Vector3 leftDir = (Quaternion.AngleAxis(halfAngle, Vector3.up) * forword).normalized;
            Vector3 rightDir = (Quaternion.AngleAxis(-halfAngle, Vector3.up) * forword).normalized;
            Gizmos.DrawLine(center, leftDir * radius + center);
            Gizmos.DrawLine(center, rightDir * radius + center);
            DrawArc(center, rightDir * radius + center, angle, radius, 40);
        }
    }

}
