using UnityEngine;

namespace SuperBilliard
{
    [RequireComponent(typeof(Rigidbody))]
    public class BiliiardSimulate : MonoBehaviour
    {
        private Rigidbody _rb;
        private MeshRenderer _meshRenderer;
        public void Stop()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.Sleep();
        }
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
        }
        public void SetMeshActive(bool flag)
        {
            _meshRenderer.enabled = flag;
        }
        public void AddForce(Vector3 dir, ForceMode forceMode)
        {
            _rb.AddForce(dir, forceMode);
        }
    }
}

