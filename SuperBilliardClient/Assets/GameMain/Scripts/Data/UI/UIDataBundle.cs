using GameFramework.DataTable;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class UIDataBundle : DataBundleBase
    {
        private readonly Dictionary<int, UIData> _dataDic = new Dictionary<int, UIData>();
        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("UIForm");
            LoadDataTable("UIGroup");
        }
        public bool HasData(int uiFormId)
        {
            return _dataDic.ContainsKey(uiFormId);
        }
        public bool TryGetData(int uiFormId, out UIData uIData)
        {
            uIData = null;
            return _dataDic.TryGetValue(uiFormId, out uIData);
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            IDataTable<DRUIForm> dRUIForms = GameEntry.DataTable.GetDataTable<DRUIForm>();
            IDataTable<DRAssetPath> dRAssetPaths = GameEntry.DataTable.GetDataTable<DRAssetPath>();
            IDataTable<DRUIGroup> dRUIGroups = GameEntry.DataTable.GetDataTable<DRUIGroup>();

            foreach (var item in dRUIForms)
            {
                DRAssetPath dRAssetPath = dRAssetPaths.GetDataRow(item.AssetId);
                DRUIGroup dRUIGroup = dRUIGroups.GetDataRow(item.UIGroupId);
                if (dRAssetPath == null || dRUIGroup == null)
                {
                    Log.Error("{0}该UIForm没有加载成功..DRUIGroup LoadFailure:{1},DRAssetPath LoadFailure:{2}.", item.Id, dRUIGroup == null, dRAssetPath == null);
                }
                else
                {
                    _dataDic.Add(item.Id, new UIData(item, dRUIGroup, dRAssetPath));
                }
            }
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            _dataDic.Clear();
        }

    }
}