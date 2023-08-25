using Knivt.Tools;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class BilliardLogicBase : EntityLogic, IBilliard
    {
        public int BilliardId
        {
            get
            {
                return BilliardData.BilliardId;
            }
        }

        public int FirestCollideId { get; protected set; }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return transform.rotation.eulerAngles;
            }
            set
            {
                transform.rotation = Quaternion.Euler(value);
            }
        }

        public float Radius
        {
            get
            {
                if (_coll == null)
                {
                    _coll = GetComponent<SphereCollider>();
                }
                return _coll.radius;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return _rigidbody.velocity;
            }
            set
            {
                _rigidbody.velocity = value;
            }
        }

        public BilliardData BilliardData
        {
            get;
            private set;
        }

        public Vector3 LastTurnPosition
        {
            get;
            private set;
        }


        protected MeshRenderer _meshRender;
        protected SphereCollider _coll;
        protected Rigidbody _rigidbody;
        //进球后的隐藏事件...
        protected float _hideTime = 0.7f;
        protected bool _hiding = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _rigidbody = GetComponent<Rigidbody>();
            _coll = GetComponent<SphereCollider>();
            _hiding = false;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            //获取数据
            BilliardData = userData as BilliardData;
            if (BilliardData == null)
            {
                Log.Error("BilliardData is invalid.");
            }
            //设置位置
            transform.position = BilliardData.Position;
            transform.rotation = BilliardData.Rotation;
            _meshRender = transform.GetComponentFromOffspring<MeshRenderer>("Modle");
            //设置材质
            _meshRender.material = GameEntry.ResourceCache.GetBilliardMaterial(BilliardData.BilliardDataItem.MaterialPath);
            BilliardManager.Instance.RigisterBilliard(this);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            //移除数据
            ReferencePool.Release(BilliardData);
        }

        public void OnSyncTransform(Vector3 positon, Vector3 euler)
        {
            Quaternion fRot = Quaternion.Euler(euler);
            transform.position = Vector3.Lerp(transform.position, positon, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, fRot, 0.2f);
        }

        public bool Decelerate(float decelerate)
        {
            if (Velocity.magnitude < 0.05f)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                if (_hiding == true)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public void SetRigidbodyEnable(bool active)
        {
            _rigidbody.isKinematic = !active;
            _coll.enabled = active;
        }

        public void TurnReset()
        {
            FirestCollideId = -1;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            //记录上一回合的位置,为了一些特殊的操作
            LastTurnPosition = transform.position;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}