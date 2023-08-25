using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class SoundData
    {
        public DRSound Sound { get; private set; }
        public DRAssetPath Path { get; private set; }
        public DRSoundPlayParam Params { get; private set; }
        public DRSoundGroup Group { get; private set; }


        public string AssetPathName => Path.AssetPath;
        public SoundData(DRAssetPath assetPath, DRSound sound, DRSoundGroup soundGroup, DRSoundPlayParam @params)
        {
            Path = assetPath;
            Sound = sound;
            Params = @params;
            Group = soundGroup;
        }
    }

    public class SoundDataBundle : DataBundleBase
    {
        private IDataTable<DRSound> _soundTable;
        private IDataTable<DRSoundGroup> _soundGroupTable;
        private IDataTable<DRSoundPlayParam> _soundPlayParamTable;
        private IDataTable<DRAssetPath> _assetPathTable;

        private readonly Dictionary<int, SoundData> _soundDateDic = new Dictionary<int, SoundData>();

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("Sound");
            LoadDataTable("SoundGroup");
            LoadDataTable("SoundPlayParam");
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _soundTable = GameEntry.DataTable.GetDataTable<DRSound>();
            _soundGroupTable = GameEntry.DataTable.GetDataTable<DRSoundGroup>();
            _soundPlayParamTable = GameEntry.DataTable.GetDataTable<DRSoundPlayParam>();
            _assetPathTable = GameEntry.DataTable.GetDataTable<DRAssetPath>();

            foreach (var sound in _soundTable)
            {
                var assetPath = _assetPathTable.GetDataRow(sound.AssetId);
                var soundGroup = _soundGroupTable.GetDataRow(sound.SoundGroupId);
                var soundPlayParam = _soundPlayParamTable.GetDataRow(sound.SoundPlayParamId);
                var soundData = new SoundData(assetPath, sound, soundGroup, soundPlayParam);
                _soundDateDic.Add(sound.Id, soundData);
            }
            //添加声音组
            foreach (var item in _soundGroupTable)
            {
                GameEntry.Sound.AddSoundGroup(item.Name, item.AvoidBeingReplacedBySamePriority, item.Mute, item.Volume, item.SoundAgentCount);
            }

            //设置对应的参数
            GameEntry.Sound.SetVolume("Music", GameEntry.Setting.GetFloat(Utility.Text.Format(SuperBilliard.Constant.GameSetting.SoundGroupVolume, "Music"), 1f));
            GameEntry.Sound.SetVolume("SFX", GameEntry.Setting.GetFloat(Utility.Text.Format(SuperBilliard.Constant.GameSetting.SoundGroupVolume, "SFX"), 1f));
            GameEntry.Sound.SetVolume("SFX/UI", GameEntry.Setting.GetFloat(Utility.Text.Format(SuperBilliard.Constant.GameSetting.SoundGroupVolume, "SFX/UI"), 1f));
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _soundTable = null;
            _soundGroupTable = null;
            _soundPlayParamTable = null;
            _assetPathTable = null;
            _soundDateDic.Clear();
        }

        public bool GetSoundData(int soundId, out SoundData soundData)
        {
            if (_soundDateDic.TryGetValue(soundId, out soundData))
            {
                return true;
            }
            return false;
        }
    }
}