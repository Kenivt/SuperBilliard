using UnityEngine;
namespace Knivt.Tools
{
    public static class VectorShortcut
    {
        public static Vector2 RotateVector2(this Vector2 v, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radian);
            float sin = Mathf.Sin(radian);

            float x = v.x * cos - v.y * sin;
            float y = v.x * sin + v.y * cos;

            return new Vector2(x, y);
        }
    }

}
