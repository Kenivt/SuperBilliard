using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class EntityLogicBase : EntityLogic
    {
        [SerializeField]
        private EntityData _entityData = null;
        public EntityData EntityData
        {
            get
            {
                return _entityData;
            }
        }
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            if (userData != null)
            {
                _entityData = userData as EntityData;
                if (_entityData == null)
                {
                    Log.Error("Entity data is invalid.");
                    return;
                }
                CachedTransform.localPosition = _entityData.Position;
                CachedTransform.localRotation = _entityData.Rotation;
                CachedTransform.localScale = Vector3.one;
            }
        }
    }
}