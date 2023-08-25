using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class NOBPlaceWhiteState : NormalOnlineBattleStateBase
    {
        private bool _showErrorTip = false;
        private LayerMask _layerMask;

        private bool _timeOut = false;
        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            _showErrorTip = true;
            _billiardManager.WhiteBilliard.Velocity = Vector3.zero;
            _layerMask = LayerMask.GetMask("Billiard");
            _timeOut = false;
            GameEntry.Event.Subscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);
            GameEntry.Event.Subscribe(TimerOutEventArgs.EventId, OnTimeOut);

            //初始化白球
            MonoBehaviour white = _billiardManager.WhiteBilliard as MonoBehaviour;
            white.gameObject.SetActive(true);
            GameEntry.Client.SendSetBilliardActiveMessage(888, true, false);
            _billiardManager.WhiteBilliard.SetRigidbodyEnable(false);
        }

        protected override void OnUpdate(IFsm<IBattleController> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            MonoBehaviour white = _billiardManager.WhiteBilliard as MonoBehaviour;
            if (white.gameObject.activeSelf == false)
            {
                white.gameObject.SetActive(true);
            }
            IBilliard whiteBilliard = _billiardManager.WhiteBilliard;
            if (whiteBilliard == null) return;

            Camera mainCamera = Camera.main;
            Vector3 pos = mainCamera.ScreenToWorldPoint(GameEntry.Input.MousePosition);
            Ray ray = mainCamera.ScreenPointToRay(GameEntry.Input.MousePosition);
            RangeType rangeType = (RangeType)fsm.GetData<VarObject>(KeyRangeType).Value;

            if (LevelManager.Instance.OnRange(pos, rangeType))
            {
                if (Physics.SphereCast(ray, whiteBilliard.Radius, out RaycastHit hitInfo, 20f, _layerMask) == false)
                {
                    Vector3 whitePos = whiteBilliard.Position;
                    whiteBilliard.Position = new Vector3(pos.x, LevelManager.Instance.BilliardYPos, pos.z);
                    if (_showErrorTip == true)
                    {
                        GameEntry.Event.Fire(this, PlaceWhiteTipEventArgs.Create(false));
                        _showErrorTip = false;
                    }
                }
                else
                {
                    if (_showErrorTip == false)
                    {
                        _showErrorTip = true;
                        GameEntry.Event.Fire(this, PlaceWhiteTipEventArgs.Create(true));
                    }
                }
            }

            if (GameEntry.Input.PlaceWhiteBall)
            {
                ChangeState<NOBPrepareState>(fsm);
            }
            else if (_timeOut)
            {
                ChangeState<NOBTimeOutState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<IBattleController> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            GameEntry.Client.SendSetBilliardActiveMessage(888, true, true);
            _billiardManager.WhiteBilliard.SetRigidbodyEnable(true);
            _billiardManager.WhiteBilliard.TurnReset();
            GameEntry.Event.Unsubscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);
            GameEntry.Event.Unsubscribe(TimerOutEventArgs.EventId, OnTimeOut);
        }

        private void OnTimeOut(object sender, GameEventArgs e)
        {
            TimerOutEventArgs args = e as TimerOutEventArgs;
            if (args == null) return;
            _timeOut = true;
        }
        private void OnSendSyncMessage(object sender, GameEventArgs e)
        {
            GameEntry.Client.SendBilliardSync(new BilliardMessage()
            {
                BilliardId = _billiardManager.WhiteBilliard.BilliardId,
                Position = _billiardManager.WhiteBilliard.Position,
                Rotation = _billiardManager.WhiteBilliard.Rotation
            });
        }
    }
}