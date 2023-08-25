using UnityEngine;
using UnityEngine.U2D;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class ResourceCacheComponent : GameFrameworkComponent
    {
        [SerializeField] private Sprite[] FancyBilliardSprites;

        public AssetLoader<Material> BilliardMaterialLoader { get; private set; } = new AssetLoader<Material>();

        public AssetLoader<SpriteAtlas> SpriteAtlasLoader { get; private set; } = new AssetLoader<SpriteAtlas>();

        public Material GetBilliardMaterial(string assetPath)
        {
            return BilliardMaterialLoader.GetAsset(assetPath);
        }

        public Sprite GetFancyBilliardSprite(int index)
        {
            if (index < 0 || index >= 16)
            {
                return null;
            }
            return FancyBilliardSprites[index];
        }
    }
}