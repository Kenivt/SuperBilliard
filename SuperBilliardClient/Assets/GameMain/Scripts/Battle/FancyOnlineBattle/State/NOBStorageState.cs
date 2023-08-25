using UnityEngine;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace SuperBilliard
{
    public sealed class NOBStorageState : NormalOnlineBattleStateBase
    {
        private float _energy = 0;
        private float _maxDistance;
        private Vector3 _diraction;
        private Vector3 _curMousePoint;
        private Vector3 _startDragMousePoint;

        private bool _timeOut = false;

        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            _energy = 0;
            _startDragMousePoint = GameEntry.Input.MousePosition;
            _diraction = -fsm.GetData<VarVector3>(KeyFireDiraction).Value;
            _maxDistance = fsm.GetData<VarSingle>(KeyMaxDragDistance).Value;
            _timeOut = false;
            GameEntry.Event.Subscribe(TimerOutEventArgs.EventId, OnTimeOut);
            if (fsm.HasData(KeyFillAmount) == false)
            {
                fsm.SetData<VarSingle>(KeyFillAmount, 1);
            }
        }



        protected override void OnUpdate(IFsm<IBattleController> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            Storage(fsm);
        }

        private void Storage(IFsm<IBattleController> fsm)
        {
            if (GameEntry.Input.LeftMouseButton)
            {
                _curMousePoint = GameEntry.Input.MousePosition;
            }
            //获得鼠标拖拽向量
            Vector3 mouseDragDir = _curMousePoint - _startDragMousePoint;
            mouseDragDir = new Vector3(mouseDragDir.x, 0, mouseDragDir.y);
            float angle = Vector3.Angle(mouseDragDir, _diraction);
            float distance = 0;
            float fillAmount = 0;

            if (angle <= fsm.GetData<VarSingle>(KeyMaxAngle))
            {
                distance = Mathf.Clamp(Vector3.Distance(_curMousePoint, _startDragMousePoint), 0, _maxDistance);
                fillAmount = distance / _maxDistance;
                GameEntry.Event.Fire(this, UpdataStorageBarEventArgs.Create(fillAmount));
                GameEntry.Event.Fire(this, CueStorageEventArgs.Create(fillAmount));
            }
            else
            {
                GameEntry.Event.Fire(this, UpdataStorageBarEventArgs.Create(0));
                GameEntry.Event.Fire(this, CueStorageEventArgs.Create(0));
            }

            //进入其他的状态
            if (GameEntry.Input.LeftMouseButtonUp)
            {
                if (fillAmount > 0.01f)
                {
                    fsm.SetData<VarSingle>(KeyFillAmount, fillAmount);
                    ChangeState<NOBRollingState>(fsm);
                }
                else
                {
                    ChangeState<NOBPrepareState>(fsm);
                }
            }
            else if (_timeOut)
            {
                //时间到了直接发射...
                fsm.SetData<VarSingle>(KeyFillAmount, fillAmount);
                ChangeState<NOBRollingState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<IBattleController> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            fsm.SetData<VarSingle>(KeyForce, _energy);
            GameEntry.Event.Fire(this, CueStorageEventArgs.Create(0));
            GameEntry.Event.Fire(this, HideBilliardIdTipEventArgs.Create());
            GameEntry.Event.Unsubscribe(TimerOutEventArgs.EventId, OnTimeOut);
        }

        private void OnTimeOut(object sender, GameEventArgs e)
        {
            _timeOut = true;
        }
    }
}