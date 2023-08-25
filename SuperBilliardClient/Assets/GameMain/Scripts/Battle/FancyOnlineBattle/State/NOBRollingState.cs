using GameMessage;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class NOBRollingState : NormalOnlineBattleStateBase
    {
        private float _timer = 0;

        private bool _syncCue = false;

        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            _timer = 0;
            LevelManager.Instance.LineRenderer.positionCount = 0;
            GameEntry.Event.Subscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);

            //发送停止计时事件
            GameEntry.Event.FireNow(this, StopTimeCountEventArgs.Create());
            GameEntry.Client.SendBattleEmptyEvent(GameMessage.BattleEmptyEvent.StopTimeCount);

            //隐藏台球杆
            _syncCue = false;
            GameEntry.Event.Fire(this, CueChangeEventArgs.Create(new Vector3(-20, 0, 11), 0));

            float force = fsm.GetData<VarSingle>(KeyFillAmount).Value * fsm.GetData<VarSingle>(KeyMaxVelocity).Value;
            _billiardManager.WhiteBilliard.TurnReset();
            _billiardManager.WhiteBilliard.Velocity = fsm.GetData<VarVector3>(KeyFireDiraction).Value.normalized * force;
        }

        protected override void OnUpdate(IFsm<IBattleController> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            _timer += elapseSeconds;
            if (_timer < 1f)
            {
                return;
            }
            bool flag = _billiardManager.AllUsingBilliardStop() && _billiardManager.WhiteBilliard.Decelerate(0);
            if (flag)
            {
                ChangeState<NOBMiddleState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<IBattleController> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.Event.Unsubscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);
            GameEntry.Client.SendBilliardSync(_billiardManager.GetAllRollingBilliardMessage().ToArray());

            var packet = CSTurnAnalysis.Create(_billiardManager.WhiteBilliard.FirestCollideId);
            GameEntry.Client.Send(packet);

            // GameEntry.Event.Fire(this, AllBilliardStopEventArgs.Create(_billiardManager.WhiteBilliard.FirestCollideId));
        }

        private void OnSendSyncMessage(object sender, GameEventArgs e)
        {
            if (_syncCue == false)
            {
                _syncCue = true;
                GameEntry.Client.SendCueSync(new Vector3(-20, 0, 11), 0);
            }
            GameEntry.Client.SendBilliardSync(_billiardManager.GetAllRollingBilliardMessage().ToArray());
        }
    }
}