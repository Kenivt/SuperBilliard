using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class PhysicSimulateComponent : UnityGameFramework.Runtime.GameFrameworkComponent
    {
        public const string SimulateSceneName = "SimulateScene";
        public Scene SimulateScene
        {
            get
            {
                if (_simulateScene == default)
                {
                    _simulateScene = SceneManager.GetSceneByName(SimulateSceneName);
                    if (_simulateScene == default)
                    {
                        _simulateScene = SceneManager.CreateScene(SimulateSceneName, new CreateSceneParameters(LocalPhysicsMode.Physics3D));
                    }
                }
                return _simulateScene;
            }
        }
        private Scene _simulateScene;


        private HashSet<GameObject> _objSet = new HashSet<GameObject>();
        private Dictionary<string, GameObject> _simulatePrefabDic = new Dictionary<string, GameObject>();

        #region 小球模拟方法
        public LineRenderer LineRenderer => _lineRenderer;
        [SerializeField] private BiliiardSimulate _billiardPrefab;
        [SerializeField] private LineRenderer _lineRenderer;
        private readonly Queue<BiliiardSimulate> _billiardQueue = new Queue<BiliiardSimulate>(2);

        public void SimulateBilliard(Vector3 force, Vector3 whitePos, Vector3 hitPoint, Vector3 targetPos, float helperLindlength, int step, float interval)
        {
            BiliiardSimulate whiteBall = GetBilliard();
            whiteBall.Stop();
            whiteBall.transform.position = whitePos;
            whiteBall.AddForce(force, ForceMode.Force);

            BiliiardSimulate targetBall = GetBilliard();
            targetBall.Stop();
            targetBall.transform.position = targetPos;

            _lineRenderer.positionCount = 4;
            _lineRenderer.SetPosition(0, whiteBall.transform.position);
            _lineRenderer.SetPosition(1, hitPoint);
            _lineRenderer.SetPosition(2, targetPos);
            PhysicsScene physicsScene = _simulateScene.GetPhysicsScene();

            for (int i = 0; i < step; i++)
            {
                if (Vector3.Distance(targetBall.transform.position, targetPos) > 0.02f)
                {
                    Vector3 dir = (targetBall.transform.position - targetPos).normalized;
                    dir.y = 0;
                    _lineRenderer.SetPosition(3, targetPos + dir * helperLindlength);
                    break;
                }
                physicsScene.Simulate(interval);
            }
            RecycleBilliard(whiteBall);
            RecycleBilliard(targetBall);
        }
        public void SetLine(Vector3 whitePosition, Vector3 endPosition)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, whitePosition);
            _lineRenderer.SetPosition(1, endPosition);
        }
        private void RecycleBilliard(BiliiardSimulate billiard)
        {
            if (_billiardQueue.Contains(billiard))
            {
                Log.Warning("重复添加...");
                return;
            }
            billiard.Stop();
            _billiardQueue.Enqueue(billiard);
        }
        private BiliiardSimulate GetBilliard()
        {
            if (_billiardQueue.Count == 0)
            {
                var obj = Instantiate(_billiardPrefab);
                BiliiardSimulate biliiard = obj.GetComponent<BiliiardSimulate>();
                biliiard.SetMeshActive(false);
                AddObj(obj.gameObject);
                _billiardQueue.Enqueue(biliiard);
            }
            BiliiardSimulate billiard = _billiardQueue.Dequeue();
            return billiard;
        }

        #endregion

        #region 注册方法
        public void AddObj(GameObject obj)
        {
            if (_objSet.Contains(obj))
            {
                return;
            }
            _objSet.Add(obj);
            SceneManager.MoveGameObjectToScene(obj, SimulateScene);
        }
        public void RemoveObj(GameObject obj)
        {
            if (!_objSet.Contains(obj))
            {
                return;
            }
            _objSet.Remove(obj);
            Destroy(obj);
        }
        public void RemoveAll()
        {
            foreach (var obj in _objSet)
            {
                Destroy(obj);
            }
            _objSet.Clear();
        }
        public bool RigisterPrefab(string key, GameObject objPrefab)
        {
            if (_simulatePrefabDic.ContainsKey(key))
            {
                Log.Warning("已经包含对应{0}的prefab", key);
                return false;
            }
            else
            {
                _simulatePrefabDic.Add("Default", objPrefab);
                return true;
            }
        }
        public bool UnrigisterPrefab(string key)
        {
            return _simulatePrefabDic.Remove(key);
        }
        public bool UnrigisterAllPrefab()
        {
            _simulatePrefabDic.Clear();
            return true;
        }
        public bool HasObj(GameObject obj)
        {
            return _objSet.Contains(obj);
        }
        #endregion

    }
}