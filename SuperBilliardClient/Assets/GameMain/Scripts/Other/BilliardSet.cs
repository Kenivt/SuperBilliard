using UnityEngine;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class BilliardSet : MonoBehaviour
    {
        public PhysicMaterial _noSpringMaterial;
        public Transform _spawnPoint;

        public readonly Queue<IBilliard> queue = new Queue<IBilliard>();
        private float _timer = 0;
        private float _time = 2f;

        private void Update()
        {
            _timer += Time.deltaTime;
            while (queue.Count > 0 && _timer > _time)
            {
                _timer = 0;
                IBilliard billiard = queue.Dequeue();
                MonoBehaviour mono = (MonoBehaviour)billiard;
                mono.gameObject.SetActive(true);
                Rigidbody rigidbody = mono.gameObject.GetComponent<Rigidbody>();
                rigidbody.isKinematic = false;
                var coll = mono.gameObject.GetComponent<Collider>();
                coll.material = _noSpringMaterial;
                coll.enabled = true;
                billiard.Position = _spawnPoint.position;
                billiard.Velocity = Vector3.back * 10f;
            }
        }

        public void AddBiliard(IBilliard billiard)
        {
            ((MonoBehaviour)billiard).gameObject.SetActive(false);
            queue.Enqueue(billiard);
        }
    }
}