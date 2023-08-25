using UnityEngine;

namespace SuperBilliard
{
    public static class VectorUtility
    {
        public static Vector3[] CreateBallArrangement(Vector3 firstBallPosition, float ballRadius, int rows)
        {
            if (rows < 0)
            {
                throw new System.Exception("行数不能小于0！！");
            }
            int ballCount = rows * (1 + rows) / 2;
            Vector3[] ballPositions = new Vector3[ballCount];
            int ballIndex = 0;
            //三角阵列
            float xOffset = Mathf.Sqrt(3) * ballRadius;
            for (int i = 0; i < rows; i++)
            {
                float zOffset = -i * ballRadius;
                for (int j = 0; j <= i; j++)
                {
                    float x = firstBallPosition.x + xOffset * i;
                    float y = firstBallPosition.y;
                    float z = firstBallPosition.z + zOffset + j * ballRadius * 2;
                    ballPositions[ballIndex] = new Vector3(x, y, z);
                    ballIndex++;
                }
            }
            return ballPositions;
        }
    }
}