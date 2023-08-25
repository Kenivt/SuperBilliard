using GameFramework.Event;

namespace SuperBilliard
{
    public class SnokkerRuleSystem : IBattleRuleSystem
    {
        public SnokkerData snokkerData;

        private System.Random positionRandom;
        //计时系统
        private float _timer = 0;
        private bool _startTimeOut = true;

        public void Init(IBattleData userData)
        {
            snokkerData = (SnokkerData)userData;
            positionRandom = userData.PostionRandom;
            snokkerData.Reset();

            GameEntry.Event.Subscribe(AllBilliardStopEventArgs.EventId, OnAllBIlliardStop);
            GameEntry.Event.Subscribe(BilliardGoalEventArgs.EventId, OnBIlliardGoal);
            //GameEntry.Event.Subscribe(StartTimeCountEventArgs.EventId, OnStartTimeCount);
            GameEntry.Event.Subscribe(StopTimeCountEventArgs.EventId, OnStopTimeCount);

            GameEntry.Event.Subscribe(StartTurnEventArgs.EventId, OnStartTurn);
            GameEntry.Event.Subscribe(EndTurnEventArgs.EventId, OnEndTurn);
        }

        public void ShutDown()
        {
            GameEntry.Event.Unsubscribe(AllBilliardStopEventArgs.EventId, OnAllBIlliardStop);
            GameEntry.Event.Unsubscribe(BilliardGoalEventArgs.EventId, OnBIlliardGoal);
            //GameEntry.Event.Unsubscribe(StartTimeCountEventArgs.EventId, OnStartTimeCount);
            GameEntry.Event.Unsubscribe(StopTimeCountEventArgs.EventId, OnStopTimeCount);

            GameEntry.Event.Unsubscribe(StartTurnEventArgs.EventId, OnStartTurn);
            GameEntry.Event.Unsubscribe(EndTurnEventArgs.EventId, OnEndTurn);
        }

        private void OnBIlliardGoal(object sender, GameEventArgs e)
        {
            BilliardGoalEventArgs args = (BilliardGoalEventArgs)e;
            snokkerData.Goal(args.Billiard.BilliardId);
        }

        private void OnAllBIlliardStop(object sender, GameEventArgs e)
        {
            AllBilliardStopEventArgs args = (AllBilliardStopEventArgs)e;
            int fristBilliard = args.FristCollideId;
            int score = snokkerData.CaculateScore(fristBilliard);
            bool writeBallEnterHole = snokkerData.WriteBallEnterHole;
            snokkerData.TurnRefersh(score, BilliardHome);
            bool ownVictor = false;
            //判断游戏是否结束
            if (snokkerData.IsGameOver(ref ownVictor, BilliardHome))
            {
                if (ownVictor)
                {
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.Victory);
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    GameEntry.Client.SendGameResult(GameMessage.GameResult.Victory);
                }
                else
                {
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.Defeated);
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    GameEntry.Client.SendGameResult(GameMessage.GameResult.Defeat);
                }
            }
            //计算分数
            if (score <= 0)
            {
                if (snokkerData.IsOwnTurn)
                {
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.Foul);
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    snokkerData.IsOwnTurn = false;
                    GameEntry.Event.Fire(this, UpdatePlayerScoreEvenrArgs.Create(false, snokkerData.OpponentScore));
                    //发送回合结束的事件
                    //发送分数变化事件
                    GameEntry.Event.Fire(this, EndTurnEventArgs.Create());
                }
                else
                {
                    snokkerData.IsOwnTurn = true;
                    //发送回合开始的事件
                    //发送分数变化事件
                    GameEntry.Event.Fire(this, UpdatePlayerScoreEvenrArgs.Create(true, snokkerData.OwnScore));
                    //开始回合...
                    GameEntry.Event.Fire(this, StartTurnEventArgs.Create(writeBallEnterHole));
                    if (writeBallEnterHole)
                    {
                        GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(BattleMessageEnumText.PlaceWhiteBall)));
                    }
                    else
                    {
                        string key = GameEntry.Localization.GetString(BattleMessageEnumText.StartTurn);
                        GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    }
                }
            }
            else
            {
                if (snokkerData.IsOwnTurn)
                {
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.Score);
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    GameEntry.Event.Fire(this, UpdatePlayerScoreEvenrArgs.Create(true, snokkerData.OwnScore));
                    GameEntry.Event.Fire(this, StartTurnEventArgs.Create(writeBallEnterHole));
                }
                else
                {
                    GameEntry.Event.Fire(this, UpdatePlayerScoreEvenrArgs.Create(false, snokkerData.OpponentScore));
                }
            }
        }

        private void StartTimeCount()
        {
            //开始计数
            _startTimeOut = true;
            _timer = 40f;
            GameEntry.Event.Fire(this, StartTimeCountEventArgs.Create(40f));
        }
        private void TurnReset()
        {
            StartTimeCount();
            BilliardManager.Instance.ResetAllUsingBilliard();
        }
        private void OnEndTurn(object sender, GameEventArgs e)
        {
            GameEntry.Event.Fire(this, UpdateActiveBilliardTypeEventArgs.Create(false, snokkerData.ActiveBallType));
            TurnReset();
        }
        private void OnStartTurn(object sender, GameEventArgs e)
        {
            StartTurnEventArgs args = (StartTurnEventArgs)e;
            GameEntry.Event.Fire(this, UpdateActiveBilliardTypeEventArgs.Create(snokkerData.IsOwnTurn, snokkerData.ActiveBallType));
            TurnReset();
        }

        //private void OnStartTimeCount(object sender, GameEventArgs e)
        //{
        //    StartTimeCountEventArgs args = (StartTimeCountEventArgs)e;
        //    _timer = args.Time;
        //    _startTimeOut = true;
        //}

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (_startTimeOut)
            {
                _timer -= realElapseSeconds;
                if (_timer < 0)
                {
                    _startTimeOut = false;
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.TimeOut);
                    GameEntry.Event.Fire(this, TimerOutEventArgs.Create());
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                }
            }
        }

        private void OnStopTimeCount(object sender, GameEventArgs e)
        {
            _startTimeOut = false;
        }

        private void BilliardHome(int id)
        {
            IBilliard target = BilliardManager.Instance.GetBilliard(id);

            //两个情况都符合后才能归位,否则重新计算位置
            if (LevelManager.Instance.IsCompliant(target.LastTurnPosition, target.Radius)
                && LevelManager.Instance.OnRange(target.LastTurnPosition, RangeType.SnokkerPlaceAngle))
            {
                target.Position = target.LastTurnPosition;
            }
            else
            {
                target.Position = LevelManager.Instance.GetRandomPosition(positionRandom, target.Radius, 100);
            }
            //添加回去...
            target.SetActive(true);
            BilliardManager.Instance.AddUsingBilliard(target);
        }

        public void Reset()
        {


        }
    }
}