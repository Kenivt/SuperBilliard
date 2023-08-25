using UnityEngine;
using GameFramework.Event;

namespace SuperBilliard
{
    public class CueLogic : EntityLogicBase
    {
        private Transform _modle;
        private float _yOrginal;

        private float _syncInterval;
        private bool _syncing = false;
        private Quaternion _syncRotation;
        private Vector3 _syncPosition;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _modle = transform.Find("Modle");
            _yOrginal = _modle.localPosition.y;
            _syncInterval = GameEntry.Config.GetFloat("SyncInterval");
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _syncing = false;
            _syncPosition = _modle.position;
            _syncRotation = transform.rotation;
            GameEntry.Event.Subscribe(CueStorageEventArgs.EventId, OnClubStorage);
            GameEntry.Event.Subscribe(CueChangeEventArgs.EventId, OnClubChange);
            GameEntry.Event.Subscribe(CueSyncEventArgs.EventId, OnCueSync);
            GameEntry.Event.Subscribe(StartSyncEventArgs.EventId, OnStartSync);
            GameEntry.Event.Subscribe(StopSyncEventArgs.EventId, OnStopSync);
        }

        private void OnStopSync(object sender, GameEventArgs e)
        {
            _syncing = false;
        }

        private void OnStartSync(object sender, GameEventArgs e)
        {
            _syncing = true;
            _syncRotation = transform.rotation;
            _syncPosition = transform.position;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (_syncing)
            {
                transform.position = Vector3.Lerp(transform.position, _syncPosition, 0.7f);
                transform.rotation = Quaternion.Lerp(transform.rotation, _syncRotation, 0.7f);
            }
        }

        private void OnClubStorage(object sender, GameEventArgs e)
        {
            CueStorageEventArgs args = e as CueStorageEventArgs;
            if (args == null)
            {
                return;
            }
            _modle.SetLocalPositionY(-args.FillAmount * args.MaxOffset + _yOrginal);
        }

        private void OnClubChange(object sender, GameEventArgs e)
        {
            CueChangeEventArgs args = e as CueChangeEventArgs;
            if (args == null)
            {
                return;
            }
            transform.position = args.Position;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, args.EulerY, transform.rotation.eulerAngles.z);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(CueStorageEventArgs.EventId, OnClubStorage);
            GameEntry.Event.Unsubscribe(CueChangeEventArgs.EventId, OnClubChange);
            GameEntry.Event.Unsubscribe(CueSyncEventArgs.EventId, OnCueSync);
            GameEntry.Event.Unsubscribe(StartSyncEventArgs.EventId, OnStartSync);
            GameEntry.Event.Unsubscribe(StopSyncEventArgs.EventId, OnStopSync);
        }

        private void OnCueSync(object sender, GameEventArgs e)
        {
            CueSyncEventArgs cueSyncEventArgs = e as CueSyncEventArgs;
            if (cueSyncEventArgs == null)
            {
                return;
            }
            _syncRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cueSyncEventArgs.angleY, transform.rotation.eulerAngles.z);
            _syncPosition = cueSyncEventArgs.position;
        }
    }
}