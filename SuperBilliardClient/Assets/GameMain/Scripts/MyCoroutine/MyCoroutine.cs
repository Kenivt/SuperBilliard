using System;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class MyCoroutineWaitUtill : IMyCoroutineItem
    {
        public bool isDone
        {
            get
            {
                if (func == null)
                {
                    return true;
                }
                else
                {
                    return func();
                }
            }
        }

        private Func<bool> func;

        public void Reset()
        {
            func = null;
        }

        public MyCoroutineWaitUtill(Func<bool> func)
        {
            this.func = func;
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {

        }
    }

    public class MyCorotineWaitSecond : IMyCoroutineItem
    {
        public bool isDone { get; private set; }

        private float _waitTime;
        private float _timer;

        public MyCorotineWaitSecond(float waitTime)
        {
            this._waitTime = waitTime;
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            _timer += elapseSeconds;
            if (_timer >= _waitTime)
            {
                isDone = true;
            }
        }

        public void Reset()
        {
            isDone = false;
            _timer = 0;
        }
    }

    public interface IMyCoroutineItem
    {
        bool isDone { get; }
        /// <summary>
        /// 为了可以重复使用
        /// </summary>
        void Reset();
        void Update(float elapseSeconds, float realElapseSeconds);
    }

    public class MyCoroutine
    {
        private readonly Dictionary<string, IEnumerator<IMyCoroutineItem>> _dic = new Dictionary<string, IEnumerator<IMyCoroutineItem>>();
        private readonly HashSet<IEnumerator<IMyCoroutineItem>> _routineSet = new HashSet<IEnumerator<IMyCoroutineItem>>();

        private readonly Queue<IEnumerator<IMyCoroutineItem>> _setDeleteQueue = new Queue<IEnumerator<IMyCoroutineItem>>();
        private readonly Queue<string> _dicDeleteQueue = new Queue<string>();

        public void StartCoroutine(IEnumerator<IMyCoroutineItem> routine)
        {
            if (_routineSet.Contains(routine))
            {
                throw new System.Exception("Coroutine already exist!");
            }
            _routineSet.Add(routine);
        }

        public void StartCoroutine(string key, IEnumerator<IMyCoroutineItem> routine)
        {
            if (_dic.ContainsKey(key))
            {
                throw new System.Exception($"Coroutine key {key} already exist!");
            }
            _dic.Add(key, routine);
        }

        public void StopCoroutine(string key, IEnumerator<IMyCoroutineItem> routine)
        {
            if (_dic.ContainsKey(key))
            {
                _dic[key].Dispose();
                _dic.Remove(key);
            }
        }
        public void StopCoroutine(IEnumerator<IMyCoroutineItem> routine)
        {
            _routineSet.Remove(routine);
            routine.Dispose();
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            UpdateSet(elapseSeconds, realElapseSeconds);
            UpdateDictionary(elapseSeconds, realElapseSeconds);
        }

        private void UpdateSet(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var item in _routineSet)
            {
                //该协程已经结束
                if (item.Current == null && item.MoveNext() == false)
                {
                    //重置
                    _setDeleteQueue.Enqueue(item);
                }
                else if (item.Current != null)
                {
                    item.Current.Update(elapseSeconds, realElapseSeconds);
                    //该项结束
                    if (item.Current.isDone)
                    {
                        item.Current.Reset();
                        if (item.MoveNext() == false)
                        {
                            _setDeleteQueue.Enqueue(item);
                        }
                    }
                }
            }
            while (_setDeleteQueue.Count > 0)
            {
                var item = _setDeleteQueue.Dequeue();
                item.Dispose();
                _routineSet.Remove(item);
            }

        }
        private void UpdateDictionary(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var item in _dic)
            {
                var value = item.Value;

                //该协程已经结束
                if (value.Current == null && value.MoveNext() == false)
                {
                    //重置
                    _dicDeleteQueue.Enqueue(item.Key);
                }
                else if (value.Current != null)
                {
                    value.Current.Update(elapseSeconds, realElapseSeconds);
                    //该项结束
                    if (value.Current.isDone)
                    {
                        value.Current.Reset();
                        if (value.MoveNext() == false)
                        {
                            _dicDeleteQueue.Enqueue(item.Key);
                        }
                    }
                }
            }
            while (_dicDeleteQueue.Count > 0)
            {
                string key = _dicDeleteQueue.Dequeue();
                _dic[key].Dispose();
                _dic.Remove(key);
            }
        }
    }
}