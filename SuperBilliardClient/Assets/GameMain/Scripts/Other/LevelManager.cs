using Knivt.Tools;
using UnityEngine;
using Knivt.Tools.AI;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public enum RangeType
    {
        Full,
        Half,
        SnokkerHalfCircle,
        SnokkerPlaceAngle,
    }

    public class LevelManager : Sington<LevelManager>
    {

        [SerializeField] private MovePath _allRange;

        [SerializeField] private MovePath _halfRange;

        [SerializeField] private MovePath _halfCircleRange;

        [SerializeField] private MovePath _placeRange;

        [SerializeField] private LayerMask _billiardLayer;
        public LineRenderer LineRenderer => _lineRenderer;
        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private Vector3 _defaultPos;

        public Vector3 DefaultPos => _defaultPos;

        public float BilliardYPos => _billiardYPos;
        [SerializeField] private float _billiardYPos;

        public bool OnRange(Vector3 position, RangeType rangeType)
        {
            bool flag = false;
            if (rangeType == RangeType.Full)
            {
                Vector3 leftBottom = _allRange.GetPos(1).Value;
                Vector3 rightTop = _allRange.GetPos(3).Value;
                flag = position.x > leftBottom.x && position.x < rightTop.x && position.z > leftBottom.z && position.z < rightTop.z;
            }
            else if (rangeType == RangeType.Half)
            {
                Vector3 leftBottom = _halfRange.GetPos(1).Value;
                Vector3 rightTop = _halfRange.GetPos(3).Value;
                flag = position.x > leftBottom.x && position.x < rightTop.x && position.z > leftBottom.z && position.z < rightTop.z;
            }
            else if (rangeType == RangeType.SnokkerHalfCircle)
            {
                Vector3 center = _halfCircleRange.GetPos(0).Value;
                float radius = Vector3.Distance(center, _halfCircleRange.GetPos(1).Value);
                position.y = center.y;
                Vector3 mouseDir = (position - center).normalized;
                float angle = Mathf.Abs(Vector3.Angle(mouseDir, Vector3.left));
                float mouseDic = Vector3.Distance(position, center);
                flag = (mouseDic <= radius) && (angle > 0 && angle < 90);
            }
            else if (rangeType == RangeType.SnokkerPlaceAngle)
            {
                Vector3 leftBottom = _placeRange.GetPos(1).Value;
                Vector3 rightTop = _placeRange.GetPos(3).Value;
                flag = position.x > leftBottom.x && position.x < rightTop.x && position.z > leftBottom.z && position.z < rightTop.z;
            }
            return flag;
        }

        //判断此点是否合规
        public bool IsCompliant(Vector3 center, float radius)
        {
            if (Physics.CheckSphere(center, radius, _billiardLayer) == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Vector3 GetRandomPosition(System.Random random, float radius, int maxFindStep)
        {
            Vector3 leftDown = _placeRange.GetPos(1).Value;
            Vector3 rightTop = _placeRange.GetPos(3).Value;

            float width = rightTop.x - leftDown.x;
            float height = rightTop.z - leftDown.z;
            //设置初始坐标
            Vector3 randomPos = Vector3.zero;
            randomPos.y = _billiardYPos;
            //防止死循环
            do
            {
                maxFindStep--;
                randomPos = new Vector3(leftDown.x + width * (float)random.NextDouble(),
                   _billiardYPos, leftDown.z + height * (float)random.NextDouble());
                if (IsCompliant(randomPos, radius))
                {
                    break;
                }
            } while (maxFindStep > 0);
            if (maxFindStep == 0)
            {
                Log.Warning("超过范围了...");
            }
            //获得随机点
            return randomPos;
        }
    }
}