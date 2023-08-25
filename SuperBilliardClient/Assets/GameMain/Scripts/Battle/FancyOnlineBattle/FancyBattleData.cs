using GameMessage;
using System;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class FancyBattleData : IBattleData
    {
        public EnumBattle BattleType
        {
            get;
            private set;
        }
        public int foulCount { get; set; }
        public BilliardType billiardType { get; set; }
        /// <summary>
        /// 台球的状态数组,如果进球则对应id的球为true                                
        /// </summary>
        public bool[] BilliardsState { get; private set; }

        public int SingleBilliardCount { get; private set; }
        public int DoubleBilliardCount { get; private set; }

        public bool IsOwnTurn { get; set; }

        public Random PostionRandom { get; private set; }

        public FancyBattleData(BattleDataItem battleDataItem)
        {
            BattleType = battleDataItem.BattleType;
            PostionRandom = new Random(battleDataItem.RandomSeed);

            BilliardsState = new bool[16];
            for (int i = 0; i < 16; i++)
            {
                BilliardsState[i] = false;
            }
            SingleBilliardCount = 7;
            DoubleBilliardCount = 7;
        }

        public void Reset()
        {
            foulCount = 0;
            billiardType = BilliardType.None;
        }

        public bool Victory()
        {
            if (billiardType == BilliardType.Single)
            {
                return SingleBilliardCount == 0 && BilliardsState[8];
            }
            else if (billiardType == BilliardType.Double)
            {
                return DoubleBilliardCount == 0 && BilliardsState[8];
            }
            else
            {
                return false;
            }
        }

        public void BilliardGoal(int ballId)
        {
            if (ballId == 0)
            {
                return;
            }

            if (ballId < 0 || ballId > 15)
            {
                Log.Error("没有对应id:{0}的球...", ballId);
                return;
            }
            if (BilliardsState[ballId])
            {
                Log.Error("为什么会重复进球？{0}", ballId);
                return;
            }
            BilliardsState[ballId] = true;


            if (ballId < 8)
            {
                SingleBilliardCount--;
            }
            else if (ballId > 8)
            {
                DoubleBilliardCount--;
            }
        }
    }
}
