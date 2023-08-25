using GameMessage;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class FancyOnlineRuleSystem : IBattleRuleSystem
    {
        private BilliardManager _billiardManager;

        private FancyBattleData _battleData;

        private bool _packetWhiteFoul = false;
        private bool _packetBlackFoul = false;
        private bool _score = false;
        private BilliardType _billiardType = BilliardType.None;
        private readonly Queue<BilliardMessage> _syncBIlliard;

        //计时系统
        private float _timecount = 0;
        private bool _startTimer = true;

        public FancyOnlineRuleSystem()
        {
            _billiardManager = BilliardManager.Instance;
            _syncBIlliard = new Queue<BilliardMessage>();
        }

        public void Init(IBattleData userdata)
        {
            _battleData = userdata as FancyBattleData;
            if (_battleData == null)
            {
                Log.Error("Battledata is invaild...");
            }

            //根据这两个事件来判断得分情况
            GameEntry.Event.Subscribe(BilliardGoalEventArgs.EventId, OnBilliardGoal);
            GameEntry.Event.Subscribe(AllBilliardStopEventArgs.EventId, OnAllBIlliardStop);

            GameEntry.Event.Subscribe(StartTurnEventArgs.EventId, OnStartTurn);

            GameEntry.Event.Subscribe(ConfirmBilliardTypeEventArgs.EventId, OnConfirmBilliardType);
            //GameEntry.Event.Subscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);

            //停止计时
            GameEntry.Event.Subscribe(StopTimeCountEventArgs.EventId, OnStopTimeCount);
            GameEntry.Event.Subscribe(EndTurnEventArgs.EventId, OnEndTurn);
        }

        public void ShutDown()
        {
            GameEntry.Event.Unsubscribe(BilliardGoalEventArgs.EventId, OnBilliardGoal);
            GameEntry.Event.Unsubscribe(AllBilliardStopEventArgs.EventId, OnAllBIlliardStop);

            GameEntry.Event.Unsubscribe(StartTurnEventArgs.EventId, OnStartTurn);
            GameEntry.Event.Unsubscribe(ConfirmBilliardTypeEventArgs.EventId, OnConfirmBilliardType);

            //GameEntry.Event.Unsubscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);
            GameEntry.Event.Unsubscribe(StopTimeCountEventArgs.EventId, OnStopTimeCount);
            GameEntry.Event.Unsubscribe(EndTurnEventArgs.EventId, OnEndTurn);
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (_startTimer == true && _battleData.IsOwnTurn)
            {
                _timecount -= elapseSeconds;
                if (_timecount < 0f)
                {
                    _startTimer = false;
                    GameEntry.Event.Fire(this, TimerOutEventArgs.Create());
                }
            }
        }

        private void OnBilliardGoal(object sender, GameEventArgs e)
        {
            BilliardGoalEventArgs args = e as BilliardGoalEventArgs;
            //进球数据
            int billiardId = args.Billiard.BilliardId;

            if (billiardId != 0)
            {
                _battleData.BilliardGoal(billiardId);
            }

            bool isOwnTurn = _battleData.IsOwnTurn;

            //判定得分情况,此情况只有是自己的回合才会记录得分情况
            if (isOwnTurn == false)
            {
                return;
            }

            //自己的回合才进行同步信息,进球之后同步一次数据,在这里重新同步一次是防止球不会入洞
            _syncBIlliard.Enqueue(new BilliardMessage()
            {
                BilliardId = billiardId,
                Position = args.Billiard.Position,
                Rotation = args.Billiard.Rotation,
            });

            if (billiardId == 0)
            {
                _packetWhiteFoul = true;
            }
            else if (billiardId == 8)
            {
                if (_battleData.billiardType == GameMessage.BilliardType.Single && _battleData.SingleBilliardCount == 0)
                {
                    _packetBlackFoul = false;
                }
                else if (_battleData.billiardType == GameMessage.BilliardType.Double && _battleData.DoubleBilliardCount == 0)
                {
                    _packetBlackFoul = false;
                }
                else
                {
                    _packetBlackFoul = true;
                }
            }
            else
            {
                //未确定球色的时候,判断进洞的球的类型是否与第一个碰撞的球的类型相同
                if (_billiardType == BilliardType.None && IsSameTypeBilliard(_billiardManager.WhiteBilliard.FirestCollideId, billiardId))
                {
                    if (billiardId < 8)
                    {
                        _billiardType = BilliardType.Single;
                    }
                    else
                    {
                        _billiardType = BilliardType.Double;
                    }
                    _score = true;
                }
                //有一个同种类型的球进洞了,则判定为得分
                if (_score == false)
                {
                    if (_battleData.billiardType == BilliardType.Single)
                    {
                        _score = billiardId > 0 && billiardId < 8;
                    }
                    else if (_battleData.billiardType == BilliardType.Double)
                    {
                        _score = billiardId > 8;
                    }
                }
            }
        }

        private void OnEndTurn(object sender, GameEventArgs e)
        {
            _battleData.IsOwnTurn = false;
            GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(BattleMessageEnumText.EndTurn)));

            StartTimeCount();
        }

        private void OnAllBIlliardStop(object sender, GameEventArgs e)
        {
            AllBilliardStopEventArgs args = e as AllBilliardStopEventArgs;
            bool isOwnTurn = _battleData.IsOwnTurn;
            int fristCollideID = args.FristCollideId;
            if (isOwnTurn == false)
            {
                return;
            }

            //判断是否进了黑8
            if (_packetBlackFoul == true)
            {
                string key = GameEntry.Localization.GetString(BattleMessageEnumText.EightEnterHole);
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                GameEntry.Client.SendGameResult(GameMessage.GameResult.Defeat);
                return;
            }

            //当得分且自己的台球类型为None的时,此时是确定球色的时候
            if (_score && _battleData.billiardType == BilliardType.None && _billiardType != BilliardType.None)
            {
                _battleData.billiardType = _billiardType;
                GameEntry.Client.SendBilliardTypeConfirme(_billiardType);
                string billiardType = _battleData.billiardType == BilliardType.Single ? "单色球" : "双色球";
                string key = GameEntry.Localization.GetString(BattleMessageEnumText.ConfirmBilliardType);
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(string.Format(key, billiardType)));
                GameEntry.Event.FireNow(this, ConfirmBilliardTypeEventArgs.Create(_battleData.billiardType));
            }

            //查看第一碰撞球是否正确
            bool firstCollideCorrect = true;

            if (_battleData.billiardType == BilliardType.Single)
            {
                if (fristCollideID == 8 && _battleData.SingleBilliardCount == 0)
                {
                    firstCollideCorrect = true;
                }
                else
                {
                    firstCollideCorrect = fristCollideID > 0 && fristCollideID < 8;
                }
            }
            else if (_battleData.billiardType == BilliardType.Double)
            {
                if (fristCollideID == 8 && _battleData.DoubleBilliardCount == 0)
                {
                    firstCollideCorrect = true;
                }
                else
                {
                    firstCollideCorrect = fristCollideID >= 8;
                }
            }
            else
            {
                firstCollideCorrect = fristCollideID > 0 && fristCollideID != 8;
            }

            //判断是否有错误
            if (_packetWhiteFoul || (firstCollideCorrect == false))
            {
                _battleData.foulCount++;

                if (_battleData.foulCount >= 5)
                {
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.Defeated);
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));

                    GameEntry.Client.SendGameResult(GameMessage.GameResult.Defeat);
                    return;
                }
                //如果判断是否胜利
                if (_battleData.Victory())
                {
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.Victory);
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));

                    GameEntry.Client.SendGameResult(GameMessage.GameResult.Victory);
                    return;
                }
                if (firstCollideCorrect == false)
                {
                    if (fristCollideID == -1)
                    {
                        string key = GameEntry.Localization.GetString(BattleMessageEnumText.WhiteNoCollde);
                        GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    }
                    else if (fristCollideID == 8)
                    {
                        string key = GameEntry.Localization.GetString(BattleMessageEnumText.EightEnterHole);
                        GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    }
                    else if (fristCollideID == -2)
                    {
                        string key = GameEntry.Localization.GetString(BattleMessageEnumText.WhiteCollideEdge);
                        GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    }
                }
                //白球错误
                if (_packetWhiteFoul)
                {
                    string key = GameEntry.Localization.GetString(BattleMessageEnumText.WhiteEnterHole);
                    GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                }
                string key2 = GameEntry.Localization.GetString(BattleMessageEnumText.FoulCountFormat);
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(string.Format(key2, _battleData.foulCount, 5)));
                GameEntry.Event.FireNow(this, EndTurnEventArgs.Create());

                GameEntry.Client.SendEndTurn(true);
            }
            else if (_score == true)
            {
                //发送胜利事件,并通知服务端
                if (_battleData.Victory())
                {
                    GameEntry.Client.SendGameResult(GameMessage.GameResult.Victory);
                }
                else
                {
                    //string key = GameEntry.Localization.GetString(BattleMessageEnumText.Score);
                    //GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(key));
                    GameEntry.Event.Fire(this, StartTurnEventArgs.Create(false));
                }
            }
            else
            {
                if (_battleData.Victory())
                {
                    GameEntry.Client.SendGameResult(GameMessage.GameResult.Victory);
                }
                else
                {
                    GameEntry.Event.FireNow(this, EndTurnEventArgs.Create());
                    GameEntry.Client.SendEndTurn(false);
                }
            }
        }

        private void StartTimeCount()
        {
            //开始计数
            _startTimer = true;
            _timecount = 40f;
            GameEntry.Event.Fire(this, StartTimeCountEventArgs.Create(40f));
        }

        private void OnConfirmBilliardType(object sender, GameEventArgs e)
        {
            if (sender == this)
            {
                return;
            }
            ConfirmBilliardTypeEventArgs args = e as ConfirmBilliardTypeEventArgs;
            if (args == null)
            {
                return;
            }
            _battleData.billiardType = args.billiardType;
            _billiardType = args.billiardType;
        }

        private void OnStartTurn(object sender, GameEventArgs e)
        {
            StartTurnEventArgs args = e as StartTurnEventArgs;
            if (args.IsPlaceWhite)
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(BattleMessageEnumText.PlaceWhiteBall)));
            }
            else
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(BattleMessageEnumText.StartTurn)));
            }
            StartTimeCount();
            //重置所有球的状态
            _billiardManager.WhiteBilliard.TurnReset();

            //判断回合结束事件
            _battleData.IsOwnTurn = true;
            _packetWhiteFoul = false;
            _packetBlackFoul = false;
            _score = false;
        }

        private void OnSendSyncMessage(object sender, GameEventArgs e)
        {
            while (_syncBIlliard.Count > 0)
            {
                GameEntry.Client.SendBilliardSync(_syncBIlliard.Dequeue());
            }
        }

        private bool IsSameTypeBilliard(int aId, int bId)
        {
            if (aId < 0 || aId >= 16)
            {
                return false;
            }
            return aId < 8 && bId < 8 || aId > 8 && bId > 8;
        }

        private void OnStopTimeCount(object sender, GameEventArgs e)
        {
            _startTimer = false;
        }

        public void Reset()
        {
            _startTimer = false;
        }
    }
}