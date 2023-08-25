using UnityEngine;
using UnityEngine.U2D;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public static class ResourceCacheComponentExtension
    {
        public static Sprite GetKitSprite(this ResourceCacheComponent component, int id)
        {
            string key = string.Format("Kit_{0}", id);
            return component.GetSprite(EnumSpriteAtlas.KitAtlas, key);
        }

        public static Sprite GetSnokkerBilliardSprite(this ResourceCacheComponent component, SnokkerBilliardType billiardType)
        {
            int id = 0;
            switch (billiardType)
            {
                case SnokkerBilliardType.Red:
                    id = 1;
                    break;
                case SnokkerBilliardType.Yellow:
                    id = 2;
                    break;
                case SnokkerBilliardType.Green:
                    id = 3;
                    break;
                case SnokkerBilliardType.Brown:
                    id = 4;
                    break;
                case SnokkerBilliardType.Blue:
                    id = 5;
                    break;
                case SnokkerBilliardType.Pink:
                    id = 6;
                    break;
                case SnokkerBilliardType.Black:
                    id = 7;
                    break;
                case SnokkerBilliardType.Colored:
                    id = 8;
                    break;
            }
            string key = $"Snokker_{id}";

            return component.GetSprite(EnumSpriteAtlas.SnokkerAtlas, key);
        }

        public static Sprite GetBIlliardSprite(this ResourceCacheComponent component, BilliardData data)
        {
            string key = data.BilliardDataItem.BilliardSpriteName;
            return component.GetSprite(data.BilliardDataItem.SpriteAtlasId, key);
        }

        public static Sprite GetBodySprite(this ResourceCacheComponent component, int id)
        {
            string key = string.Format("Body_{0}", id);
            return component.GetSprite(EnumSpriteAtlas.BodyAtlas, key);
        }

        public static Sprite GetFaceSprite(this ResourceCacheComponent component, int id)
        {
            string key = string.Format("Face_{0}", id);
            return component.GetSprite(EnumSpriteAtlas.FaceAtlas, key);
        }

        public static Sprite GetHairSprite(this ResourceCacheComponent component, int id)
        {
            string key = string.Format("Hair_{0}", id);
            return component.GetSprite(EnumSpriteAtlas.HairAtlas, key);
        }

        public static Sprite GetSprite(this ResourceCacheComponent component, EnumSpriteAtlas enumSpriteAtlas, string key)
        {
            return component.GetSprite((int)enumSpriteAtlas, key);
        }

        public static Sprite GetSprite(this ResourceCacheComponent component, int targetId, string key)
        {
            SpriteAtlasDataBundle bundle = GameEntry.DataBundle.GetData<SpriteAtlasDataBundle>();
            string path = bundle.GetSpriteData(targetId).Path;
            SpriteAtlas spriteAtlas = component.SpriteAtlasLoader.GetAsset(path);
            if (spriteAtlas == null)
            {
                Log.Warning("SpriteAtlas is null, path is {0}", path);
                return null;
            }
            return spriteAtlas.GetSprite(key);
        }
    }
}