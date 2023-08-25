using System;
using UnityEngine;
using GameFramework;

namespace SuperBilliard
{
    [Serializable]
    public class EntityData : IReference
    {
        protected int _serilzieId = 0;

        protected Vector3 position = Vector3.zero;

        protected Quaternion rotation = Quaternion.identity;

        public EntityData()
        {

        }

        /// <summary>
        /// 实体编号
        /// </summary>
        public int Id
        {
            get
            {
                return _serilzieId;
            }
        }

        /// <summary>
        /// 实体位置。
        /// </summary>

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public static EntityData Create(int serilizeId, Vector3 position, Quaternion rotation)
        {
            EntityData entityData = ReferencePool.Acquire<EntityData>();
            entityData._serilzieId = serilizeId;
            entityData.position = position;
            entityData.rotation = rotation;
            return entityData;
        }


        public virtual void Clear()
        {
            _serilzieId = 0;
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }
    }
}