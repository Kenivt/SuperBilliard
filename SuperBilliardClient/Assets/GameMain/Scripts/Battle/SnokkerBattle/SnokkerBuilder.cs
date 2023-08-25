using System;
using UnityEngine;
using GameFramework.Event;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SnokkerBuilder : IBattleBuilder
    {
        public bool IsComplete { get; private set; }

        public event Action OnComplete;

        private readonly MyCoroutine _myCoroutine = new MyCoroutine();

        private readonly Dictionary<int, bool> _uiLoadFlags = new Dictionary<int, bool>();

        private readonly Dictionary<int, bool> _entityLoadFlags = new Dictionary<int, bool>();

        public void Build(BattleDataItem battleItem)
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            //开始加载协程
            _myCoroutine.StartCoroutine(LoadGame(battleItem));
        }

        IEnumerator<IMyCoroutineItem> LoadGame(BattleDataItem battleItem)
        {
            //初始化一下
            IsComplete = false;
            _entityLoadFlags.Clear();
            _uiLoadFlags.Clear();
            //加载台球
            BilliardDataBundle dataBundle = GameEntry.DataBundle.GetData<BilliardDataBundle>();

            for (int i = 0; i < battleItem.BilliardIdArray.Length; i++)
            {
                BilliardDataItem billiardDataItem = dataBundle.GetBilliardData(battleItem.BilliardIdArray[i]);
                int serilizeId = GameEntry.Entity.GenerateSerialId();

                GameEntry.Entity.ShowBilliard<SnokkerBilliardLogic>(EnumEntity.SnokkerBilliard,
                    BilliardData.Create(billiardDataItem, serilizeId));
                _entityLoadFlags.Add(serilizeId, false);
            }

            //加载球杆
            CueData cueData = new CueData(GameEntry.Entity.GenerateSerialId());
            //放到下侧用来隐藏球杆
            cueData.Position = new Vector3(0, -2, 0);
            cueData.Rotation = Quaternion.Euler(0, 180, 100);
            GameEntry.Entity.ShowCue(cueData);
            _entityLoadFlags.Add(cueData.Id, false);

            //加载UI
            int? uiId = GameEntry.UI.OpenUIForm(EnumUIForm.SnokkerPlayerDataUIForm);

            if (uiId != null)
            {
                _uiLoadFlags.Add(uiId.Value, false);
            }
            else
            {
                Log.Error("加载UI失败");
            }

            yield return new MyCoroutineWaitUtill(() =>
            {
                foreach (var item in _entityLoadFlags)
                {
                    if (!item.Value)
                    {
                        return false;
                    }
                }
                foreach (var item in _uiLoadFlags)
                {
                    if (!item.Value)
                    {
                        return false;
                    }
                }
                return true;
            });

            IsComplete = true;
            OnComplete?.Invoke();
            //取消订阅
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public void Update(float elapseSeconds, float realelapseSeconds)
        {
            _myCoroutine.Update(elapseSeconds, realelapseSeconds);
        }

        public void Reset()
        {
            //关闭所有加载过的物品
            //GameEntry.UI.CloseUIForm(EnumUIForm.BattleTipUIForm);
            foreach (var item in _uiLoadFlags)
            {
                GameEntry.UI.CloseUIForm(item.Key);
            }
            foreach (var item in _entityLoadFlags)
            {
                GameEntry.Entity.HideEntity(item.Key);
            }
            //然后再重新Build
            IsComplete = false;
        }

        //加载对应的资源
        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
            OpenUIFormFailureEventArgs args = (OpenUIFormFailureEventArgs)e;
            if (_uiLoadFlags.ContainsKey(args.SerialId))
            {
                Log.Error("加载UI失败,对应的UI为{0}", args.UIFormAssetName);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs args = (OpenUIFormSuccessEventArgs)e;
            if (_uiLoadFlags.ContainsKey(args.UIForm.SerialId))
            {
                _uiLoadFlags[args.UIForm.SerialId] = true;
            }
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs args = e as ShowEntitySuccessEventArgs;

            int id = args.Entity.Id;
            if (_entityLoadFlags.ContainsKey(id))
            {
                _entityLoadFlags[id] = true;
            }
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs args = e as ShowEntityFailureEventArgs;
            int id = args.EntityId;
            if (_entityLoadFlags.ContainsKey(id))
            {
                Log.Error("Show entity failure with error message '{0}'.", args.ErrorMessage);
            }
        }

    }
}