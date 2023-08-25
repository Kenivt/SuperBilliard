using UnityEngine;
using UnityGameFramework;

namespace SuperBilliard
{
    public class UIData
    {
        public DRUIForm DrUIForm { get; private set; }
        public DRUIGroup UIGroup { get; private set; }
        public DRAssetPath DrAssetPath { get; private set; }

        public string AssetName => DrAssetPath.AssetPath;
        private string UIGroupName => UIGroup.Name;

        public UIData(DRUIForm dRUIForm, DRUIGroup dRUIGroup, DRAssetPath assetPath)
        {
            DrUIForm = dRUIForm;
            UIGroup = dRUIGroup;
            DrAssetPath = assetPath;
        }
    }
}