using System;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class SceneDataItem
    {
        public DRScene drScene;
        public DRAssetPath drAssetPath;
        public SceneDataItem(DRScene drScene, DRAssetPath drAssetPath)
        {
            this.drScene = drScene;
            this.drAssetPath = drAssetPath;
        }
    }
    public class SceneDataBundle : DataBundleBase
    {
        private readonly Dictionary<int, SceneDataItem> _dataItemDic = new Dictionary<int, SceneDataItem>();

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("Scene");
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            IDataTable<DRScene> dataTable = GameEntry.DataTable.GetDataTable<DRScene>();
            IDataTable<DRAssetPath> assetPathTable = GameEntry.DataTable.GetDataTable<DRAssetPath>();
            foreach (var item in dataTable)
            {
                assetPathTable = GameEntry.DataTable.GetDataTable<DRAssetPath>();
                DRAssetPath drAssetPath = assetPathTable.GetDataRow(item.AssetId);
                _dataItemDic.Add(item.Id, new SceneDataItem(item, drAssetPath));
            }
        }
        public string GetAssetPath(int id)
        {
            if (_dataItemDic.ContainsKey(id))
            {
                return _dataItemDic[id].drAssetPath.AssetPath;
            }
            return null;
        }
        public Type GetProcedureType(int id)
        {
            if (_dataItemDic.ContainsKey(id))
            {
                string typeName = _dataItemDic[id].drScene.Procedure;
                typeName = "SuperBilliard." + typeName;
                Type type = Type.GetType(typeName);
                if (type == null)
                {
                    throw new Exception($"没有找到类型为{typeName}的Procedure");
                }
                return type;
            }
            else
            {
                throw new Exception($"没有找到id为{id}的场景数据");
            }
        }
    }
}
