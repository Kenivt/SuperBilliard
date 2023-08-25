using GameFramework.Fsm;
using SuperBilliard;
using System.Collections.Generic;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameEntry = SuperBilliard.GameEntry;
using GameFramework.Event;
using System;

public class ResourcePreloadTest : GameFramework.Procedure.ProcedureBase
{
    private string[] _dataTables = new string[]
    {
        "AssetPath",
        "Entity",
        "EntityGroup",
        "PoolParam"
    };
    private Dictionary<string, bool> _loadedFlag = new Dictionary<string, bool>();
    protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);
    }
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, LoadDataTableSuccessCallback);
        GameEntry.Event.Subscribe(LoadDictionaryFailureEventArgs.EventId, LoadDataTableFailureCallback);
        _loadedFlag.Clear();
        for (int i = 0; i < _dataTables.Length; i++)
        {
            LoadDataTable(_dataTables[i]);
        }
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
        Log.Info("进入ProcedureTest中");
        ChangeState<ProcedureTest>(procedureOwner);
    }
    private void LoadDataTable(string dataTableName)
    {
        string dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, true);
        _loadedFlag.Add(dataTableAssetName, false);
        GameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        GameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, LoadDataTableSuccessCallback);
        GameEntry.Event.Unsubscribe(LoadDictionaryFailureEventArgs.EventId, LoadDataTableFailureCallback);
    }
}
