using GameFramework.Fsm;
using GameFramework.Event;

namespace SuperBilliard
{
    public class NOBOpponentTurnState : NormalOnlineBattleStateBase
    {
        private bool _placeWhite = false;
        private bool _changeOwnTurn = false;

        public BilliardMessage[] _billiardMessages = new BilliardMessage[22];

        protected override void OnInit(IFsm<IBattleController> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            _changeOwnTurn = false;
            //发送同步信息
            GameEntry.Event.Subscribe(StartTurnEventArgs.EventId, StartTurnCallback);
            GameEntry.Event.Subscribe(SyncBilliardEventArgs.EventId, SyncBilliardCallback);
            //发送开始同步事件
            GameEntry.Event.FireNow(this, StartSyncEventArgs.Create());

            //重新设置...
            for (int i = 0; i < _billiardMessages.Length; i++)
            {
                _billiardMessages[i].needSync = false;
            }
        }

        protected override void OnUpdate(IFsm<IBattleController> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            //同步台球信息
            for (int i = 0; i < _billiardMessages.Length; i++)
            {
                if (_billiardMessages[i].needSync == false)
                {
                    continue;
                }
                BilliardManager.Instance.SyncUsingBilliard(i, _billiardMessages[i].Position, _billiardMessages[i].Rotation);
            }

            if (_changeOwnTurn)
            {
                _changeOwnTurn = false;
                if (_placeWhite == true)
                    ChangeState<NOBPlaceWhiteState>(fsm);
                else
                    ChangeState<NOBPrepareState>(fsm);
            }
        }

        private void SyncBilliardCallback(object sender, GameEventArgs e)
        {
            SyncBilliardEventArgs args = e as SyncBilliardEventArgs;
            if (args == null)
            {
                return;
            }

            //获取需要同步的台球信息
            for (int i = 0; i < _billiardMessages.Length; i++)
            {
                _billiardMessages[i].needSync = false;
            }

            foreach (var item in args.BilliardMessageList)
            {
                _billiardMessages[item.BilliardId].needSync = true;
                _billiardMessages[item.BilliardId].Position = item.Position;
                _billiardMessages[item.BilliardId].Rotation = item.Rotation;
            }
        }

        private void StartTurnCallback(object sender, GameEventArgs e)
        {
            StartTurnEventArgs args = e as StartTurnEventArgs;
            if (args == null)
            {
                return;
            }
            _changeOwnTurn = true;
            _placeWhite = args.IsPlaceWhite;
        }

        protected override void OnLeave(IFsm<IBattleController> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            GameEntry.Event.FireNow(this, StopSyncEventArgs.Create());
            GameEntry.Event.Unsubscribe(StartTurnEventArgs.EventId, StartTurnCallback);
            GameEntry.Event.Unsubscribe(SyncBilliardEventArgs.EventId, SyncBilliardCallback);
        }
    }
}