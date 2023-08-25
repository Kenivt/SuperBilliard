using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    /// <summary>
    /// 0号为白球 , 1~15 为红球 1分 16黄球-2分 17绿球-3分 
    /// 18棕球-4分 19蓝球-5分 20粉球-6分 21黑球-7分
    /// 此枚举类型是对应球的类型上方为得分详解
    /// </summary>
    public enum SnokkerBilliardType
    {
        White = 1 << 0,
        Red = 1 << 1,
        Yellow = 1 << 2,
        Green = 1 << 3,
        Brown = 1 << 4,
        Blue = 1 << 5,
        Pink = 1 << 6,
        Black = 1 << 7,
        Colored = 0X00FC
    }

    /// <summary>
    /// 斯诺克的数据
    /// </summary>
    public class SnokkerData : IBattleData
    {
        public EnumBattle BattleType
        {
            get;
            private set;
        }

        public SnokkerData(EnumBattle enumBattle, int roomSeed)
        {
            BattleType = enumBattle;
            //设置随机种子
            PostionRandom = new Random(roomSeed);
        }

        //当前是否是我方的回合
        public bool IsOwnTurn
        {
            get;
            set;
        }

        /// <summary>
        /// 敌方得分
        /// </summary>
        public int OpponentScore
        {
            get
            {
                return _opponentScore;
            }
            set
            {
                //发送事件
                _opponentScore = value;
            }
        }

        /// <summary>
        /// 我方得分
        /// </summary>
        public int OwnScore
        {
            get
            {
                return _ownScore;
            }
            set
            {
                //发送事件
                _ownScore = value;
            }
        }

        /// <summary>
        /// 红球的数量
        /// </summary>
        public int RedBallCount
        {
            get
            {
                return _redBallCount;
            }
        }

        /// <summary>
        /// 活动球的类型
        /// </summary>
        public SnokkerBilliardType ActiveBallType
        {
            get;
            private set;
        }

        private int _ownScore;

        private int _opponentScore;

        //红球的数量
        private int _redBallCount;

        //用来记录所有台球的状态
        private bool[] _billiardsState = new bool[22];

        //用来记录当前回合进球的台球Id
        private readonly List<int> _goalBilliardIdList = new List<int>();
        public bool WriteBallEnterHole => _billiardsState[0];

        public Random PostionRandom { get; private set; }

        /// <summary>
        /// 台球进洞
        /// </summary>
        /// <param name="id">对应台球的Id</param>
        public void Goal(int id)
        {
            if (id < 0 || id >= _billiardsState.Length)
            {
                Console.WriteLine("台球的Id不合法,对应ID为:{0}", id);
                return;
            }
            //台球已经进过洞了
            if (_billiardsState[id])
            {
                Console.WriteLine("对应{0}的台球已经进过洞了", id);
                return;
            }
            Log.Warning("{0}..进球", id);
            //记录红球的数量
            SnokkerBilliardType snokkerBilliardType = GetSnokkerBilliardType(id);
            if (snokkerBilliardType == SnokkerBilliardType.Red)
            {
                _redBallCount--;
            }
            _billiardsState[id] = true;
            _goalBilliardIdList.Add(id);

            snokkerBilliardType.ToString();
        }
        /// <summary>
        /// 计算当前的得分
        /// </summary>
        /// <returns>返回值为得分,得分为负数则说明犯规了...</returns>
        public int CaculateScore(int fristCollideId)
        {
            int score = 0;
            bool foul = false;

            Log.Info("第一碰撞球" + fristCollideId);
            for (int i = 0; i < _goalBilliardIdList.Count; i++)
            {
                Log.Info("进球的ID" + _goalBilliardIdList[i]);
            }
            SnokkerBilliardType firstCollideBilliardType = GetSnokkerBilliardType(fristCollideId);
            //判断是否有碰撞错误
            if (fristCollideId < 0)
            {
                score = -4;
                foul = true;
            }//如果不是活动球
            else if (IsSameColor(firstCollideBilliardType) == false)
            {
                int billiardScore = GetScore(fristCollideId);
                foul = true;
                score = Math.Min(-4, -billiardScore);
            }

            //接下来是对进球的判断
            foreach (var id in _goalBilliardIdList)
            {
                int billiardScore = GetScore(id);
                SnokkerBilliardType theType = GetSnokkerBilliardType(id);

                //这时候严格判断类型是否相同了
                if (theType != firstCollideBilliardType)
                {
                    foul = true;
                    //获取得分
                    score = Math.Min(score, -4);
                    score = Math.Min(score, -billiardScore);
                }
                else if (foul == false)
                {
                    score += billiardScore;
                }
            }

            if (score < 0)
            {
                if (IsOwnTurn)
                {
                    OpponentScore -= score;
                }
                else
                {
                    OwnScore -= score;
                }
            }
            else
            {
                if (IsOwnTurn)
                {
                    OwnScore += score;
                }
                else
                {
                    OpponentScore += score;
                }
            }

            return score;
        }
        /// <summary>
        /// 回合刷新
        /// </summary>
        /// <param name="score">此回合的得分</param>
        /// <param name="homeCallback">归为回调</param>
        /// <returns></returns>
        public bool TurnRefersh(int score, Action<int> homeCallback)
        {
            _goalBilliardIdList.Clear();

            //防止白球入洞
            if (_billiardsState[0] == true)
            {
                _billiardsState[0] = false;
            }
            bool flag = false;
            //得分了,可以连续进行游戏
            if (score > 0)
            {
                //处理边界条件
                if (RedBallCount > 0)
                {
                    AllBilliardHome(homeCallback);
                    ActiveBallType = ActiveBallType == SnokkerBilliardType.Red ? SnokkerBilliardType.Colored : SnokkerBilliardType.Red;
                }
                else if (ActiveBallType == SnokkerBilliardType.Red)
                {
                    ActiveBallType = SnokkerBilliardType.Colored;
                    AllBilliardHome(homeCallback);
                }
                else if (ActiveBallType == SnokkerBilliardType.Colored)
                {
                    AllBilliardHome(homeCallback);
                    flag = RefeshActiveBIlliard();
                }
                else
                {
                    flag = RefeshActiveBIlliard();
                }
            }
            else
            {
                //是否所有红球进洞
                if (RedBallCount > 0)
                {
                    AllBilliardHome(homeCallback);
                    ActiveBallType = SnokkerBilliardType.Red;
                }
                else
                {
                    SameBIlliardHome(homeCallback);
                    flag = RefeshActiveBIlliard();
                }
            }
            return flag;
        }

        public int GetColeredBilliardId(SnokkerBilliardType snokkerBilliardType)
        {
            int id = -1;
            switch (snokkerBilliardType)
            {
                case SnokkerBilliardType.Yellow:
                    id = 16;
                    break;
                case SnokkerBilliardType.Green:
                    id = 17;
                    break;
                case SnokkerBilliardType.Brown:
                    id = 18;
                    break;
                case SnokkerBilliardType.Blue:
                    id = 19;
                    break;
                case SnokkerBilliardType.Pink:
                    id = 20;
                    break;
                case SnokkerBilliardType.Black:
                    id = 21;
                    break;
                case SnokkerBilliardType.Colored:
                    id = 22;
                    break;
            }
            return id;
        }
        public static SnokkerBilliardType GetSnokkerBilliardType(int id)
        {
            if (id == 0)
            {
                return SnokkerBilliardType.White;
            }
            else if (id >= 1 && id <= 15)
            {
                return SnokkerBilliardType.Red;
            }
            else if (id >= 16 && id <= 21)
            {
                //黄球id为16,对应的类型为 1 << 3
                return (SnokkerBilliardType)(1 << (id - 14));
            }
            return SnokkerBilliardType.White;
        }

        public static int GetScore(int id)
        {
            if (id >= 1 && id <= 15)
            {
                return 1;
            }
            else if (id >= 16 && id <= 21)
            {
                return id - 14;
            }
            return 0;
        }

        public void Reset()
        {
            OwnScore = 0;
            OpponentScore = 0;
            ActiveBallType = SnokkerBilliardType.Red;
            _redBallCount = 15;
            for (int i = 0; i < _billiardsState.Length; i++)
            {
                _billiardsState[i] = false;
            }
            _goalBilliardIdList.Clear();
        }

        public bool IsGameOver(ref bool ownVictory, Action<int> homeBilliard)
        {
            if (_redBallCount > 0)
            {
                return false;
            }

            bool allBilliardGoal = true;
            for (int i = 16; i < _billiardsState.Length; i++)
            {
                if (_billiardsState[i] == false)
                {
                    allBilliardGoal = false;
                    break;
                }
            }

            //所有的球都进洞后...
            if (allBilliardGoal)
            {
                //决胜球...
                if (OwnScore == OpponentScore)
                {
                    ActiveBallType = SnokkerBilliardType.Black;
                    int index = GetColeredBilliardId(SnokkerBilliardType.Black);
                    _billiardsState[index] = false;
                    homeBilliard?.Invoke(index);
                    return false;
                }

                ownVictory = OwnScore > OpponentScore;
                return true;
            }
            else
            {
                //分数相差太大则停止游戏
                if (Math.Abs(OpponentScore - OwnScore) >= 40)
                {
                    ownVictory = OwnScore > OpponentScore;
                    return true;
                }
            }
            return false;
        }
        private void AllBilliardHome(Action<int> homeCallback)
        {
            for (int i = 16; i < _billiardsState.Length; i++)
            {
                if (_billiardsState[i] == true)
                {
                    homeCallback?.Invoke(i);
                    _billiardsState[i] = false;
                }
            }
        }

        private void SameBIlliardHome(Action<int> homeCallback)
        {
            int id = GetColeredBilliardId(ActiveBallType);
            if (id == -1)
            {
                Log.Error("警告...");
                //发送一个警告
                return;
            }
            if (id < 16)
            {
                Log.Error("为什么会有红球？");
                return;
            }
            for (int i = id + 1; i < _billiardsState.Length; i++)
            {
                if (_billiardsState[i] == true)
                {
                    _billiardsState[i] = false;
                    homeCallback?.Invoke(i);
                }
            }
        }


        /// <summary>
        /// 按照球的分值刷新活动球,求并返回是不是所有球都进洞了
        /// </summary>
        ///<return> 是否所有的球都进洞了</return>
        private bool RefeshActiveBIlliard()
        {
            //所有的红球都进洞了,查找彩球
            for (int i = 16; i < _billiardsState.Length; i++)
            {
                //找到目标活动的彩球
                if (_billiardsState[i] == false)
                {
                    ActiveBallType = GetSnokkerBilliardType(i);
                    return false;
                }
            }
            return true;
        }

        private bool IsSameColor(SnokkerBilliardType ownType)
        {
            return (ActiveBallType & ownType) == ownType;
        }
    }
}