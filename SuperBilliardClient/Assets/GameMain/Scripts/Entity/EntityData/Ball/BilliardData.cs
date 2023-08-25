using UnityEngine;
using GameFramework;

namespace SuperBilliard
{
    public class BilliardData : EntityData
    {
        protected int _ballId;
        protected int _firstCollideID;

        public BilliardDataItem BilliardDataItem { get; private set; }

        public int BilliardId
        {
            get
            {
                return BilliardDataItem.BilliardId;
            }
        }

        public override void Clear()
        {
            base.Clear();
            _ballId = 0;
            _firstCollideID = 0;
            BilliardDataItem = null;
        }

        public static BilliardData Create(BilliardDataItem billiardDataItem, int serilizeId)
        {
            BilliardData billiardData = ReferencePool.Acquire<BilliardData>();
            billiardData._serilzieId = serilizeId;
            billiardData.position = billiardDataItem.dRBilliard.Position;
            billiardData.rotation = Quaternion.Euler(billiardDataItem.dRBilliard.EulerAngle);
            billiardData.BilliardDataItem = billiardDataItem;
            return billiardData;
        }
    }
}