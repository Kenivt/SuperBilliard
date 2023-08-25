using System;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public static class EntityExtension
    {
        private static int _serialId;

        public static void ShowCue(this EntityComponent entityComponen, CueData data)
        {
            ShowEntity<CueLogic>(entityComponen, (int)EnumEntity.Cue, Constant.AssetPriority.Cue, data);
        }

        public static void ShowBilliard<T>(this EntityComponent entityComponen, EnumEntity billiardType, BilliardData data) where T : BilliardLogicBase
        {
            ShowEntity<T>(entityComponen, (int)billiardType, Constant.AssetPriority.Billiard, data);
        }

        public static void ShowEntity<T>(this EntityComponent entityComponen, int entityId, int priority, EntityData data)
        {
            ShowEntity(entityComponen, entityId, typeof(T), priority, data);
        }

        public static void ShowEntity(this EntityComponent entityComponent, int entityId, Type logicType, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }
            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            IDataTable<DRAssetPath> dRAssetPaths = GameEntry.DataTable.GetDataTable<DRAssetPath>();
            IDataTable<DREntityGroup> dREntityGroups = GameEntry.DataTable.GetDataTable<DREntityGroup>();

            DREntity drEntity = dtEntity.GetDataRow(entityId);
            DREntityGroup dREntityGroup = dREntityGroups.GetDataRow(drEntity.EntityGroupId);
            string entityGroupName = dREntityGroup.Name;
            string assetPath = dRAssetPaths.GetDataRow(drEntity.AssetId).AssetPath;

            if (!entityComponent.HasEntityGroup(entityGroupName))
            {
                IDataTable<DRPoolParam> dRPoolParams = GameEntry.DataTable.GetDataTable<DRPoolParam>();
                DRPoolParam dRPoolParam = dRPoolParams.GetDataRow(dREntityGroup.PoolParamId);
                entityComponent.AddEntityGroup(entityGroupName, dRPoolParam.InstanceAutoReleaseInterval, dRPoolParam.InstanceCapacity, dRPoolParam.InstanceExpireTime, dRPoolParam.InstancePriority);
            }
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", entityId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, assetPath, entityGroupName, priority, data);
        }
        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return _serialId++;
        }
    }
}