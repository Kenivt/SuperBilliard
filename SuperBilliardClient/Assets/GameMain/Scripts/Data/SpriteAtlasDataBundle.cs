using GameFramework.DataTable;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class SpriteAtlasData
    {
        public int Id => DRSpriteAtlas.Id;

        public DRSpriteAtlas DRSpriteAtlas
        {
            get;
            private set;
        }

        public DRAssetPath DRAssetPath
        {
            get;
            private set;
        }

        public string Path => DRAssetPath.AssetPath;

        public SpriteAtlasData(DRSpriteAtlas dRSpriteAtlas, DRAssetPath dRAssetPath)
        {
            this.DRSpriteAtlas = dRSpriteAtlas;
            this.DRAssetPath = dRAssetPath;
        }
    }

    public class SpriteAtlasDataBundle : DataBundleBase
    {
        private readonly Dictionary<int, SpriteAtlasData> _dataDic = new Dictionary<int, SpriteAtlasData>();

        public SpriteAtlasData this[EnumSpriteAtlas enumSpriteAtlas]
        {
            get
            {
                return _dataDic[(int)enumSpriteAtlas];
            }
        }

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("SpriteAtlas");
        }
        public SpriteAtlasData GetSpriteData(int id)
        {
            return _dataDic[id];
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            IDataTable<DRAssetPath> dRAssetPaths = GameEntry.DataTable.GetDataTable<DRAssetPath>();
            IDataTable<DRSpriteAtlas> dRSpriteAtlas = GameEntry.DataTable.GetDataTable<DRSpriteAtlas>();

            foreach (var item in dRSpriteAtlas)
            {
                DRAssetPath dRAssetPath = dRAssetPaths.GetDataRow(item.AssetPathId);
                SpriteAtlasData spriteAtlasData = new SpriteAtlasData(item, dRAssetPath);
                _dataDic.Add(item.Id, spriteAtlasData);
            }
        }
    }
}