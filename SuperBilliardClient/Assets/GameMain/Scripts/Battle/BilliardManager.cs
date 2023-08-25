using UnityEngine;
using System.Linq;
using Knivt.Tools;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class BilliardManager : Sington<BilliardManager>
    {
        public IBilliard WhiteBilliard
        {
            get
            {
                return _billiardDic[0];
            }
        }

        public float Deceleration { get; set; } = 0f;

        private readonly LinkedList<IBilliard> _usingBilliardList = new LinkedList<IBilliard>();

        private readonly Dictionary<int, IBilliard> _billiardDic = new Dictionary<int, IBilliard>();

        public int Count => _billiardDic.Count;
        //移除小球
        [SerializeField] private BilliardSet _billliardSet;
        [SerializeField] private PhysicMaterial _defaultMaterial;

        //加载完成游戏初始化
        public void Init()
        {
            _usingBilliardList.Clear();
            foreach (var item in _billiardDic)
            {
                _usingBilliardList.AddLast(item.Value);
            }
        }

        public void AddUsingBilliard(IBilliard billiard)
        {
            if (_usingBilliardList.Contains(billiard))
            {
                Log.Warning("一个台球被注册多次！！！");
                return;
            }
            _usingBilliardList.AddLast(billiard);
        }

        public void RemoveAllUsingBilliard()
        {
            _usingBilliardList.Clear();
        }

        public bool RemoveUsingBilliard(IBilliard billiard)
        {
            bool flag = _usingBilliardList.Remove(billiard);
            if (flag == true && _billliardSet != null)
            {
                _billliardSet.AddBiliard(billiard);
            }
            return flag;
        }

        public bool AllUsingBilliardStop()
        {
            foreach (var item in _usingBilliardList)
            {
                if (item.Decelerate(Deceleration) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public void ResetAllUsingBilliard()
        {
            foreach (var item in _usingBilliardList)
            {
                item.TurnReset();
            }
        }

        public IBilliard GetBilliard(int id)
        {
            if (_billiardDic.ContainsKey(id))
            {
                return _billiardDic[id];
            }
            else
            {
                Log.Error("没有对应Id'{0}'的台球", id);
                return null;
            }
        }

        public void SyncUsingBilliard(int ballId, Vector3 position, Vector3 rotation)
        {
            IBilliard target = _usingBilliardList.SingleOrDefault(a => { return a.BilliardId == ballId; });
            if (target == null)
            {
                Log.Warning("同步的台球不存在！！！");
                return;
            }
            target.OnSyncTransform(position, rotation);
        }

        public BilliardMessage[] GetAllUsingBilliardMessage()
        {
            BilliardMessage[] billiardMessages = new BilliardMessage[_usingBilliardList.Count];
            int index = 0;
            foreach (var item in _usingBilliardList)
            {
                billiardMessages[index].BilliardId = item.BilliardId;
                billiardMessages[index].Position = item.Position;
                billiardMessages[index].Rotation = item.Rotation;
                index++;
            }
            return billiardMessages;
        }

        public List<BilliardMessage> GetAllRollingBilliardMessage()
        {
            List<BilliardMessage> rollingBilliards = new List<BilliardMessage>();
            foreach (var item in _usingBilliardList)
            {
                if (item.Decelerate(0))
                {
                    continue;
                }
                rollingBilliards.Add(new BilliardMessage()
                {
                    BilliardId = item.BilliardId,
                    Position = item.Position,
                    Rotation = item.Rotation
                });
            }
            return rollingBilliards;
        }

        public void RigisterBilliard(IBilliard billiard)
        {
            if (_billiardDic.ContainsKey(billiard.BilliardId))
            {
                Log.Warning("一个台球被注册多次！！！");
                return;
            }
            MonoBehaviour monoBehaviour = (MonoBehaviour)billiard;
            monoBehaviour.GetComponent<SphereCollider>().material = _defaultMaterial;
            _billiardDic.Add(billiard.BilliardId, billiard);
        }

        public void UnrigisterAllBilliard()
        {
            _billiardDic.Clear();
        }

        public void SetUsingBIlliardRigidbodyEnable(bool isEnable)
        {
            foreach (var item in _usingBilliardList)
            {
                item.SetRigidbodyEnable(isEnable);
            }
        }
    }
}