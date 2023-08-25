using GameFramework.Fsm;
using GameFramework.Event;

namespace SuperBilliard
{
    public class NOBMiddleState : NormalOnlineBattleStateBase
    {
        private bool startTurn;
        private bool endTurn;
        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            startTurn = false;
            endTurn = false;
            GameEntry.Event.Subscribe(EndTurnEventArgs.EventId, OnEndTurn);
            GameEntry.Event.Subscribe(StartTurnEventArgs.EventId, OnStartTurn);
        }
        protected override void OnUpdate(IFsm<IBattleController> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (startTurn)
            {
                ChangeState<NOBPrepareState>(fsm);
            }
            else if (endTurn)
            {
                ChangeState<NOBOpponentTurnState>(fsm);
            }
        }
        protected override void OnLeave(IFsm<IBattleController> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);

            GameEntry.Event.Unsubscribe(EndTurnEventArgs.EventId, OnEndTurn);
            GameEntry.Event.Unsubscribe(StartTurnEventArgs.EventId, OnStartTurn);
        }
        private void OnStartTurn(object sender, GameEventArgs e)
        {
            StartTurnEventArgs args = e as StartTurnEventArgs;
            if (args == null)
            {
                return;
            }
            startTurn = true;
        }
        private void OnEndTurn(object sender, GameEventArgs e)
        {
            endTurn = true;
        }
    }
}