using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class RandomTest : MonoBehaviour
    {
        int seed = 0;
        [SerializeField] private GameObject _prefab;
        private Queue<GameObject> _queue = new Queue<GameObject>();
        private Queue<GameObject> _other = new Queue<GameObject>();
        private void Start()
        {

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(Test());
            }
        }

        System.Collections.IEnumerator Test()
        {
            seed = UnityEngine.Random.Range(1546313, 2658483);
            System.Random rand1 = new System.Random(1546313);
            System.Random rand2 = new System.Random(1546313);
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            for (int i = 0; i < 300; i++)
            {
                var pos1 = LevelManager.Instance.GetRandomPosition(rand1, 0.25f, 300);
                //var pos2 = LevelManager.Instance.GetRandomPosition(rand2, 0.25f, 100);
                if (_queue.TryDequeue(out GameObject o) == false)
                {
                    o = Instantiate(_prefab, transform);
                }
                o.transform.position = pos1;
                o.GetComponent<Rigidbody>().Sleep();
                o.GetComponent<MeshRenderer>().material.color = color;
                _other.Enqueue(o);
                yield return null;
            }
            yield return new WaitForSeconds(5f);

            while (_other.Count > 0)
            {
                Destroy(_other.Dequeue());
            }
        }
    }
}