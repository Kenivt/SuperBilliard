using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Knivt.Tools
{
    public class EnemyDetector : MonoBehaviour
    {
        [Header("范围参数")]
        public float viewDistance;
        public float angle;
        public List<Collider> TargetColliders => _colliders;
        private List<Collider> _colliders = new List<Collider>();
        [Header("敌人的相关信息")]
        public string targetTag;
        public float _chaseDistance;
        public LayerMask _barriarLayerMask;
        public float _checkTargetInterval;

        public Vector3 targetPosOffset;
        public Transform Target
        {
            get
            {
                if (_targetQueue.Count == 0)
                {
                    return null;
                }
                return _targetQueue.Peek();
            }
        }

        private Queue<Transform> _targetQueue = new Queue<Transform>();
        private Queue<Transform> _waitQueue = new Queue<Transform>();
        private HashSet<Transform> _targetSet = new HashSet<Transform>();

        private float _timer;
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < _checkTargetInterval)
            {
                return;
            }
            _timer = 0;
            int count = _waitQueue.Count;
            for (int i = 0; i < count; i++)
            {
                Transform target = _waitQueue.Dequeue();

                Debug.Log(PhysicUtility.OnAngleRange(target.position, transform.position, viewDistance, transform.forward, angle));
                if (PhysicUtility.OnAngleRange(target.position, transform.position, viewDistance, transform.forward, angle)
                    && PhysicUtility.CheckBarrier(transform.position, target.position + targetPosOffset, 0.5f, viewDistance, _barriarLayerMask, true) == false)
                {
                    _targetQueue.Enqueue(target);
                }
                else if (Vector3.Distance(transform.position, target.position) < _chaseDistance)
                {
                    _waitQueue.Enqueue(target);
                }
                else
                {
                    _targetSet.Remove(target);
                }
            }
            count = _targetQueue.Count;
            for (int i = 0; i < count; i++)
            {
                Transform target = _targetQueue.Dequeue();

                if (Vector3.Distance(transform.position, target.position) < _chaseDistance)
                {
                    _targetQueue.Enqueue(target);
                }
                else
                {
                    _targetSet.Remove(target);
                }
            }
            Debug.Log(_waitQueue.Count());
            Debug.Log(_targetQueue.Count());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                if (!_targetSet.Contains(other.transform))
                {
                    _targetSet.Add(other.transform);

                    if (PhysicUtility.OnAngleRange(other.transform.position, transform.position, viewDistance, transform.forward, angle)
                    && PhysicUtility.CheckBarrier(transform.position, other.transform.position + targetPosOffset, 0.5f, viewDistance, _barriarLayerMask, true) == false)
                    {
                        _targetQueue.Enqueue(other.transform);
                    }
                    else
                    {
                        _waitQueue.Enqueue(other.transform);
                    }
                }
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            GizmoUtility.DrawViewRange(transform.position, transform.forward, angle, viewDistance);
        }
    }
}