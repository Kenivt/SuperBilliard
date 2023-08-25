using GameMessage;
using GameFramework.Event;
using System.Collections.Generic;

namespace SuperBilliard
{
    public abstract class DefaultBattleBase : IBattle
    {
        public BattleDataItem Battle { get; private set; }
        public BattleState State { get; set; }

        protected IBattleData _battleData;
        protected IBattleController _battleController;
        protected IBattleRuleSystem _battleRuleSystem;
        protected IBattleBuilder _battleBuilder;

        protected MyCoroutine _myCoroutine = new MyCoroutine();
        public void Init(BattleDataItem battle)
        {
            Battle = battle;

            //获取对应的数据
            SetData(battle, ref _battleData, ref _battleController, ref _battleRuleSystem, ref _battleBuilder);

            _battleData.IsOwnTurn = battle.IsFristMove;
            GameEntry.Event.Subscribe(GameStartEventArgs.EventId, OnGameStart);
            _battleBuilder.Build(Battle);
            _battleBuilder.OnComplete += OnComplete;
            State = BattleState.Playing;
            //初始化数据
            _battleController.Init(_battleData);
            _battleRuleSystem.Init(_battleData);
        }

        public void ShutDown()
        {
            _battleController.ShutDown();
            _battleRuleSystem.ShutDown();
            GameEntry.Event.Unsubscribe(GameStartEventArgs.EventId, OnGameStart);
        }

        public void OnComplete()
        {
            //发送加载游戏完成的通知...
            BilliardManager.Instance.Init();
            _battleBuilder.OnComplete -= OnComplete;

            GameEntry.Client.Send(CSLoadGameComplete.Create());
        }

        public void Update(float elaspSeconds, float realElaspSeconds)
        {
            if (_battleBuilder.IsComplete == false)
            {
                _battleBuilder.Update(elaspSeconds, realElaspSeconds);
                return;
            }

            _myCoroutine.Update(elaspSeconds, realElaspSeconds);

            //游戏开始后...
            if (State == BattleState.Playing)
            {
                _battleRuleSystem.Update(elaspSeconds, realElaspSeconds);
            }
        }

        private void OnGameStart(object sender, GameEventArgs e)
        {
            GameStartEventArgs args = (GameStartEventArgs)e;
            State = BattleState.Playing;
            //游戏开始后延迟两秒开始游戏,为了配合开始UI
            _myCoroutine.StartCoroutine(DelayStart(args.DelayStartTime));
        }

        private IEnumerator<IMyCoroutineItem> DelayStart(float delaytime)
        {
            yield return new MyCorotineWaitSecond(delaytime);
            //正式开启游戏
            if (_battleData.IsOwnTurn)
            {
                _battleController.StartPlaceWhiteBall();
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create(GameEntry.Localization.GetString(BattleMessageEnumText.PlaceWhiteBall)));
                GameEntry.Event.Fire(this, StartTurnEventArgs.Create(true));
            }
            else
            {
                _battleController.StartOpponentTurn();
                GameEntry.Event.Fire(this, EndTurnEventArgs.Create());
            }
        }

        public void Restart()
        {
            //重新开始
            //TODO:重新开始游戏功能
            //_battleData.Reset();
            //_battleRuleSystem.Reset();
            //_battleController.Reset();
            //_battleBuilder.Reset();
        }

        protected abstract void SetData(BattleDataItem battle,
            ref IBattleData battleData, ref IBattleController battleController, ref IBattleRuleSystem ruleSystem, ref IBattleBuilder battleBuilder);
    }
}
