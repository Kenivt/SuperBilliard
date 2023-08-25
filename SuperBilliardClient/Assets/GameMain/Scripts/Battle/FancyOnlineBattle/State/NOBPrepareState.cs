using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class NOBPrepareState : NormalOnlineBattleStateBase
    {
        private Camera _mainCamera;

        private bool showBilliardIdTip = false;

        private IBilliard _whiteBilliard;
        private Vector3 _diraction;

        private float _angle = 0;
        private LayerMask _billiardLayer;
        private LayerMask _billiardEdgeLayer;

        private bool _timeout = false;

        protected override void OnInit(IFsm<IBattleController> fsm)
        {
            base.OnInit(fsm);
            _billiardLayer = LayerMask.GetMask("Billiard");
            _billiardEdgeLayer = LayerMask.GetMask("Edge", "Billiard");
        }

        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            _mainCamera = Camera.main;
            _whiteBilliard = _billiardManager.WhiteBilliard;
            _whiteBilliard.SetActive(true);
            _whiteBilliard.SetRigidbodyEnable(true);
            _timeout = false;

            GameEntry.Client.SendBilliardSync(new BilliardMessage
            {
                BilliardId = _billiardManager.WhiteBilliard.BilliardId,
                Position = _billiardManager.WhiteBilliard.Position,
                Rotation = _billiardManager.WhiteBilliard.Rotation,
            });
            GameEntry.Event.Subscribe(TimerOutEventArgs.EventId, OnTimeOut);
            GameEntry.Event.Subscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);
            GameEntry.Event.Fire(this, CueChangeEventArgs.Create(_billiardManager.WhiteBilliard.Position, _angle));

            if (fsm.HasData(KeyFireDiraction) == false)
            {
                fsm.SetData<VarVector3>(KeyFireDiraction, Vector3.zero);
            }
        }

        protected override void OnUpdate(IFsm<IBattleController> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            DisplayBilliardMessage();
            DisplayBilliardTrack();
            DisplayCue();
            if (GameEntry.Input.LeftMouseButtonDown)
            {
                ChangeState<NOBStorageState>(fsm);
            }
            else if (_timeout)
            {
                ChangeState<NOBTimeOutState>(fsm);
            }
        }

        protected override void OnLeave(IFsm<IBattleController> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            fsm.SetData<VarVector3>(KeyFireDiraction, _diraction);
            Debug.DrawLine(_whiteBilliard.Position, _whiteBilliard.Position + _diraction * 10, Color.red, 10f);
            GameEntry.Event.Unsubscribe(SendSyncMessageEventArgs.EventId, OnSendSyncMessage);
            GameEntry.Event.Unsubscribe(TimerOutEventArgs.EventId, OnTimeOut);
        }

        private void OnTimeOut(object sender, GameEventArgs e)
        {
            _timeout = true;
        }
        private void DisplayCue()
        {
            //计算球杆的角度
            _angle = Vector3.Angle(_diraction, Vector3.left);
            if (_diraction.z < 0) _angle = -_angle;

            GameEntry.Event.Fire(this, CueChangeEventArgs.Create(_whiteBilliard.Position, _angle));
        }

        private void DisplayBilliardTrack()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 whitePos = _whiteBilliard.Position;
            mouseWorldPos.y = whitePos.y;
            _diraction = (mouseWorldPos - whitePos).normalized;
            //球状射线检测...
            LineRenderer lineRenderer = LevelManager.Instance.LineRenderer;
            if (Physics.SphereCast(whitePos, _whiteBilliard.Radius, _diraction, out RaycastHit hitInfo, 20f, _billiardEdgeLayer))
            {
                if (hitInfo.transform.TryGetComponent(out IBilliard billiard))
                {
                    //如果是球，那么就显示球的轨迹
                    lineRenderer.positionCount = 3;
                    lineRenderer.SetPosition(0, whitePos);
                    lineRenderer.SetPosition(1, hitInfo.point);
                    lineRenderer.SetPosition(2, hitInfo.point + -hitInfo.normal * 1.2f);
                }
                else
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, whitePos);
                    lineRenderer.SetPosition(1, hitInfo.point);
                }
            }
        }


        private void DisplayBilliardMessage()
        {
            var ray = _mainCamera.ScreenPointToRay(GameEntry.Input.MousePosition);

            if (Physics.SphereCast(ray, _whiteBilliard.Radius, out RaycastHit hitInfo, 20f, _billiardLayer))
            {
                IBilliard billiard = hitInfo.transform.GetComponent<IBilliard>();
                GameEntry.Event.Fire(this, ShowBilliardIdTipEventArgs.Create(billiard.BilliardData));
                showBilliardIdTip = true;
            }
            else
            {
                if (showBilliardIdTip == true)
                {
                    GameEntry.Event.Fire(this, HideBilliardIdTipEventArgs.Create());
                }
                showBilliardIdTip = false;
            }
        }

        private void OnSendSyncMessage(object sender, GameEventArgs e)
        {
            GameEntry.Client.SendCueSync(_whiteBilliard.Position, _angle);
        }
    }
}