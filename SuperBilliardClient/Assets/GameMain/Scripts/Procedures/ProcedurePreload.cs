using GameFramework.Fsm;
using GameFramework.Data;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SuperBilliard
{
    public class ProcedurePreload : ProcedureBase
    {
        private DataBundleBase[] _dataBundles = null;

        public override bool UseNativeDialog => true;

        private const string ConfigName = "DefaultConfig";

        private readonly string[] _dataTables = new string[]
        {
            "AssetPath",
            "Entity",
            "EntityGroup",
            "PoolParam",
        };

        private readonly Dictionary<string, bool> _loadedFlag = new Dictionary<string, bool>();

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, LoadDataTableSuccessCallback);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, LoadDataTableFailureCallback);
            GameEntry.Event.Subscribe(LoadConfigSuccessEventArgs.EventId, LoadConfigSuccessCallback);
            GameEntry.Event.Subscribe(LoadConfigFailureEventArgs.EventId, LoadConfigFailureCallback);
            GameEntry.Event.Subscribe(LoadDictionarySuccessEventArgs.EventId, LoadDictionarySuccessCallback);
            GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, LoadDictionaryFailureCallback);
            _loadedFlag.Clear();
            for (int i = 0; i < _dataTables.Length; i++)
            {
                LoadDataTable(_dataTables[i]);
            }

            Data[] datas = GameEntry.DataBundle.GetAllData();
            _dataBundles = new DataBundleBase[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                _dataBundles[i] = datas[i] as DataBundleBase;
                if (_dataBundles[i] == null)
                {
                    throw new System.Exception($"错误,{datas[i].Name}没有继承于DataBundleBase.");
                }
            }
            LoadConfig();
            LoadDictionary("Default");
            GameEntry.DataBundle.PreLoadAllData();
        }



        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            foreach (var item in _loadedFlag)
            {
                if (item.Value == false)
                {
                    return;
                }
            }
            foreach (var item in _dataBundles)
            {
                if (item.IsPreloadReady == false)
                {
                    return;
                }
            }
            GameEntry.DataBundle.LoadAllData();
            procedureOwner.SetData<VarInt32>(KeyNextSceneId, (int)Constant.SceneId.Login);
            //procedureOwner.SetData<VarInt32>(KeyNextSceneId, (int)Constant.SceneId.FancyBilliard);
            //procedureOwner.SetData<VarInt32>(KeyBattle, (int)BattleId.FancyBilliardStandaloneBattle);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, LoadDataTableSuccessCallback);
            GameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, LoadDataTableFailureCallback);
            GameEntry.Event.Unsubscribe(LoadConfigSuccessEventArgs.EventId, LoadConfigSuccessCallback);
            GameEntry.Event.Unsubscribe(LoadConfigFailureEventArgs.EventId, LoadConfigFailureCallback);
            GameEntry.Event.Unsubscribe(LoadDictionarySuccessEventArgs.EventId, LoadDictionarySuccessCallback);
            GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, LoadDictionaryFailureCallback);
        }
        private void LoadDictionary(string assetName)
        {
            string dictionaryAssetName = AssetUtility.GetDictionaryAsset(assetName, false);
            Debug.Log(dictionaryAssetName);
            _loadedFlag.Add(dictionaryAssetName, false);
            GameEntry.Localization.ReadData(dictionaryAssetName, this);
        }

        private void LoadConfig()
        {
            string configAssetName = AssetUtility.GetConfigAsset(ConfigName, false);
            _loadedFlag.Add(configAssetName, false);
            GameEntry.Config.ReadData(configAssetName, this);
        }

        private void LoadDataTable(string dataTableName)
        {
            string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, true);
            _loadedFlag.Add(dataTableAssetName, false);
            GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
        }

        private void LoadConfigSuccessCallback(object sender, GameEventArgs e)
        {
            LoadConfigSuccessEventArgs loadConfigSuccessCallback = e as LoadConfigSuccessEventArgs;
            if (loadConfigSuccessCallback.UserData != this)
            {
                return;
            }
            string configAssetName = AssetUtility.GetConfigAsset(ConfigName, false);
            if (loadConfigSuccessCallback.ConfigAssetName == configAssetName)
            {
                Log.Info("Load Config Success!!");
                _loadedFlag[configAssetName] = true;
            }
        }

        private void LoadDictionarySuccessCallback(object sender, GameEventArgs e)
        {
            LoadDictionarySuccessEventArgs args = e as LoadDictionarySuccessEventArgs;
            if (args.UserData != this)
            {
                return;
            }
            _loadedFlag[args.DictionaryAssetName] = true;
            Log.Info("Load {0} dictionary success!!", args.DictionaryAssetName);
        }
        private void LoadDictionaryFailureCallback(object sender, GameEventArgs e)
        {
            LoadDictionaryFailureEventArgs args = e as LoadDictionaryFailureEventArgs;
            if (args.UserData != this)
            {
                return;
            }
            Log.Error("Load Dictionary Failure!!,message:{0}", args.ErrorMessage);
        }

        private void LoadConfigFailureCallback(object sender, GameEventArgs e)
        {
            Log.Error("Load Config Failure!!");
        }

        private void LoadDataTableFailureCallback(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs args = e as LoadDataTableSuccessEventArgs;
            if (args.UserData != this)
            {
                return;
            }
            Log.Error("Load {0} datatable failure!!", args.DataTableAssetName);
        }

        private void LoadDataTableSuccessCallback(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs args = e as LoadDataTableSuccessEventArgs;
            if (args.UserData != this)
            {
                return;
            }
            _loadedFlag[args.DataTableAssetName] = true;
        }
    }
}
