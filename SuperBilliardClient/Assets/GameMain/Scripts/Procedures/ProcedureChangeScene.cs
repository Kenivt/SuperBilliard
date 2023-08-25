using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private bool _loadSceneFlag = false;
        public override bool UseNativeDialog => true;
        private System.Type _procedureType = null;

        private Queue<int> _closeUIQueue = new Queue<int>();

        protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _loadSceneFlag = false;
            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneCallBack);

            //卸载所有的场景
            string[] sceneNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            foreach (var item in sceneNames)
            {
                GameEntry.Scene.UnloadScene(item);
            }

            //关闭没有忽略的UI
            CloseUI();

            //卸载所有的Entity
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Entity.HideAllLoadingEntities();

            //加载场景
            int sceneId = procedureOwner.GetData<VarInt32>(KeyNextSceneId);
            SceneDataBundle sceneDataBundle = GameEntry.DataBundle.GetData<SceneDataBundle>();
            string assetPath = sceneDataBundle.GetAssetPath(sceneId);
            _procedureType = sceneDataBundle.GetProcedureType(sceneId);
            GameEntry.Scene.LoadScene(assetPath, this);
        }

        protected override void OnLeave(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, LoadSceneCallBack);

            //清除所有的UI
            _ignoreUIQueue.Clear();
            while (_splashUIQueue.Count > 0)
            {
                GameEntry.UI.CloseUIForm(_splashUIQueue.Dequeue());
            }
        }

        protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (_loadSceneFlag)
            {
                ChangeState(procedureOwner, _procedureType);
            }
        }
        private void LoadSceneCallBack(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            _loadSceneFlag = true;
        }

        private void CloseUI()
        {
            _closeUIQueue.Clear();

            //关闭所有的UI
            foreach (var item in GameEntry.UI.GetAllLoadedUIForms())
            {
                if (_splashUIQueue.Contains(item.SerialId) || _ignoreUIQueue.Contains(item.SerialId))
                {
                    continue;
                }
                _closeUIQueue.Enqueue(item.SerialId);
            }
            foreach (var item in GameEntry.UI.GetAllLoadingUIFormSerialIds())
            {
                if (_splashUIQueue.Contains(item) || _ignoreUIQueue.Contains(item))
                {
                    continue;
                }
                _closeUIQueue.Enqueue(item);
            }
            while (_closeUIQueue.Count > 0)
            {
                GameEntry.UI.CloseUIForm(_closeUIQueue.Dequeue());
            }
        }
    }
}

