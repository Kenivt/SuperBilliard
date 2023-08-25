using System;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class BattleDataItem
    {
        public DRBattle DRBattle { get; private set; }
        public DRScene DRScene { get; private set; }
        public DRAssetPath ScenePath { get; private set; }

        public int[] BilliardIdArray => DRBattle.BilliardIdArray;

        public Dictionary<int, BilliardData> BilliardDic { get; set; }

        public EnumBattle BattleType => (EnumBattle)DRBattle.Id;
        public int SceneId => DRBattle.SceneId;

        public bool IsFristMove { get; set; }
        public int RandomSeed { get; set; }

        public BattleDataItem(DRScene dRScene, DRBattle dRBattle,
            DRAssetPath dRAssetPath)
        {
            DRBattle = dRBattle;
            DRScene = dRScene;
            ScenePath = dRAssetPath;
            RandomSeed = 114514;
        }
    }

    public class BattleDataBundle : DataBundleBase
    {
        private IDataTable<DRBattle> _battleTable;
        private Dictionary<int, BattleDataItem> _battleDataItems = new Dictionary<int, BattleDataItem>();

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("Battle");
        }

        public int GetSceneId(EnumBattle enumBattle)
        {
            return _battleTable.GetDataRow((int)enumBattle).SceneId;
        }

        public Type GetBattleType(int battleId)
        {
            string typeName = _battleTable.GetDataRow(battleId).BattleTypeName;
            typeName = "SuperBilliard." + typeName;
            return Type.GetType(typeName);
        }

        public bool HasData(int battleId)
        {
            return _battleTable.HasDataRow(battleId);
        }

        public BattleDataItem GetBattleDataItem(int battleId)
        {
            if (_battleDataItems.TryGetValue(battleId, out BattleDataItem item))
            {
                return item;
            }

            Log.Error("Can not find battle data item by id {0}", battleId);
            return null;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _battleTable = GameEntry.DataTable.GetDataTable<DRBattle>();
            IDataTable<DRAssetPath> assetPathTable = GameEntry.DataTable.GetDataTable<DRAssetPath>();
            IDataTable<DRBilliard> billiardTable = GameEntry.DataTable.GetDataTable<DRBilliard>();
            IDataTable<DRScene> scentTable = GameEntry.DataTable.GetDataTable<DRScene>();

            foreach (var item in _battleTable)
            {
                DRAssetPath dRAssetPath = assetPathTable.GetDataRow(item.SceneId);
                var dRScene = scentTable.GetDataRow(item.SceneId);
                BattleDataItem battleDataItem = new BattleDataItem(dRScene, item, dRAssetPath);
                _battleDataItems.Add(item.Id, battleDataItem);
            }
        }
    }
}