using System;
using UnityEngine;
using GameFramework.Event;
using System.Collections;

namespace SuperBilliard
{
    [Obsolete]
    public class FancyBilliard : MonoBehaviour, IBilliard
    {
        public int BilliardId
        {
            get
            {
                return _billiardId;
            }
        }

        public int FirestCollideId { get; private set; }

        public bool CanFireEvent { get; set; }

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

        public bool Syncing
        {
            get
            {
                return _isSyncing;
            }
            set
            {
                _isSyncing = value;
                if (_isSyncing == true)
                {
                    OnStartSync(null, null);
                }
                else
                {
                    OnStopSync(null, null);
                }
            }
        }

        public Vector3 LastTurnPosition => throw new NotImplementedException();

        public BilliardData BilliardData => throw new NotImplementedException();

        private SphereCollider _coll;
        private Rigidbody _rigidbody;
        [SerializeField] private int _billiardId;

        //同步的相关代码
        private bool _isSyncing = false;
        private Vector3 _syncPos;
        private Quaternion _syncRotation;

        //进球后的隐藏事件...
        private float _hideTime = 0.7f;
        private bool _hadHide = false;
        //是否正在隐藏,即进球后的隐藏时间内小球还不能停止运动
        [SerializeField] private bool _hiding = false;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _coll = GetComponent<SphereCollider>();
            _hadHide = false;
            _hiding = false;
        }

        private void OnStopSync(object sender, GameEventArgs e)
        {
            _isSyncing = false;
            _rigidbody.isKinematic = false;
        }

        private void OnStartSync(object sender, GameEventArgs e)
        {
            _isSyncing = true;
            _syncPos = Position;
            _syncRotation = transform.rotation;
            _rigidbody.isKinematic = true;
        }

        private void Update()
        {
            if (_isSyncing && !_hadHide)
            {
                transform.position = Vector3.Lerp(transform.position, _syncPos, 0.2f);
                transform.rotation = Quaternion.Lerp(transform.rotation, _syncRotation, 0.2f);
            }
        }

        public bool Decelerate(float decelerate)
        {
            if (Velocity.magnitude < 0.1f)
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

        public void TurnReset()
        {
            FirestCollideId = -1;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        public void OnSyncTransform(Vector3 positon, Vector3 euler)
        {
            _syncPos = positon;
            _syncRotation = Quaternion.Euler(euler);
        }


        private void OnCollisionEnter(Collision collision)
        {
            //不等于-1说明还没有参与碰撞

            if (FirestCollideId != -1)
            {
                return;
            }

            if (collision.gameObject.CompareTag(Constant.GameobjectTag.CannotReboundTag))
            {
                FirestCollideId = -2;
            }
            else if (collision.gameObject.CompareTag(Constant.GameobjectTag.BallTag))
            {
                FirestCollideId = collision.gameObject.GetComponent<IBilliard>().BilliardId;
            }

            //如果没有处于同步状态,则播放音效
            if (_isSyncing == false)
            {
                float volume = Mathf.Clamp01(Velocity.magnitude / 30f);
                if (volume > 0.05f)
                {
                    GameEntry.Sound.PlaySound(EnumSound.BilliardCollide, transform.position, volume);
                    GameEntry.Client.SendSyncSoundMessage(EnumSound.BilliardCollide, volume, transform.position);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag(Constant.GameobjectTag.BallHoleTag))
            {
                StopAllCoroutines();
                StartCoroutine(HideBilliard(other.transform.position));
            }
        }

        private IEnumerator HideBilliard(Vector3 holePos)
        {
            float timer = 0;
            _hiding = true;
            GameEntry.Event.FireNow(this, BilliardGoalEventArgs.Create(this));
            _rigidbody.velocity = Vector3.zero;
            while (timer < _hideTime)
            {
                timer += Time.deltaTime;
                float fillamount = Mathf.Clamp01(timer / _hideTime);
                transform.position = Vector3.Lerp(transform.position, holePos, fillamount);
                yield return null;
            }
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _hiding = false;
            //先发送进球事件,在隐藏
            if (BilliardId == 0)
            {
                Vector3 pos = Vector3.zero;
                pos.y = LevelManager.Instance.BilliardYPos;
                gameObject.transform.position = pos;
                gameObject.SetActive(false);
            }
            else
            {
                //停止同步
                _hadHide = true;
                BilliardManager.Instance.RemoveUsingBilliard(this);
            }
        }

        public void SetRigidbodyEnable(bool active)
        {
            _rigidbody.isKinematic = !active;
            _coll.enabled = active;
        }

        public void SetActive(bool active)
        {
            throw new NotImplementedException();
        }
    }
}