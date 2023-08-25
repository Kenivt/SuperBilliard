using GameFramework.DataTable;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class BilliardDataItem
    {
        public DRBilliard dRBilliard;
        public DRAssetPath materialAssetPath;

        public int BilliardId => dRBilliard.BilliardId;

        public string MaterialPath => materialAssetPath.AssetPath;

        public string BilliardSpriteName => dRBilliard.BilliardName;

        public int SpriteAtlasId => dRBilliard.SpriteAtlasId;

        public BilliardDataItem(DRBilliard dRBilliard, DRAssetPath assetPath)
        {
            this.dRBilliard = dRBilliard;
            this.materialAssetPath = assetPath;
        }
    }

    public class BilliardDataBundle : DataBundleBase
    {
        private readonly Dictionary<int, BilliardDataItem> _dic = new Dictionary<int, BilliardDataItem>();

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("Billiard");
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            IDataTable<DRBilliard> table = GameEntry.DataTable.GetDataTable<DRBilliard>();
            IDataTable<DRAssetPath> assetPathTable = GameEntry.DataTable.GetDataTable<DRAssetPath>();

            foreach (DRBilliard dRBilliard in table.GetAllDataRows())
            {
                DRAssetPath assetPath = assetPathTable.GetDataRow(dRBilliard.MaterialAssetId);
                if (assetPath == null)
                {
                    Log.Warning("Can not load material asset path '{0}' from data table.", dRBilliard.MaterialAssetId.ToString());
                    continue;
                }
                _dic.Add(dRBilliard.Id, new BilliardDataItem(dRBilliard, assetPath));
            }
        }

        public BilliardDataItem GetBilliardData(int billiardId)
        {
            BilliardDataItem dataItem = null;
            if (_dic.TryGetValue(billiardId, out dataItem))
            {
                return dataItem;
            }
            return null;
        }

        public void GetAllBilliardData(List<BilliardDataItem> list)
        {
            list.Clear();
            foreach (var item in _dic)
            {
                list.Add(item.Value);
            }
        }
    }
}