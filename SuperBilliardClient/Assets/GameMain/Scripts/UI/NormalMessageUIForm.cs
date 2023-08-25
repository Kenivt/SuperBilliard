using UnityEngine;
using GameFramework.Event;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class NormalMessageUIForm : UILogicBase
    {
        [SerializeField] private Message tipPrefabs;
        [SerializeField] private RectTransform _root;
        [SerializeField] private RectTransform _down;
        [SerializeField] private RectTransform _up;

        [SerializeField, Tooltip("当短时间内大量显示信息的时候，显示信息的间隔")] private float _showMessageInterval;

        private Queue<string> _waitQueue = new Queue<string>();
        private Queue<Message> _rycycleTipQueue = new Queue<Message>();
        public LinkedList<Message> _linkedList = new LinkedList<Message>();

        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private AnimationCurve _alphaCurve;
        private float _timer = 0;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            eventSubscriber.Subscribe(ShowMessageEventArgs.EventId, ShowMessageCallback);
        }

        private void ShowMessageCallback(object sender, GameEventArgs e)
        {
            ShowMessageEventArgs args = (ShowMessageEventArgs)e;
            _waitQueue.Enqueue(args.Message);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            eventSubscriber.UnSubscribeAll();

            ClearWaitQueue();
            foreach (var item in _linkedList)
            {
                item.ResetMessage();
                ReleaseTip(item);
            }
            _linkedList.Clear();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //进入等待队列
            _timer += elapseSeconds;
            if (_timer > _showMessageInterval)
            {
                _timer = 0;
                if (_waitQueue.Count > 0)
                {
                    string message = _waitQueue.Dequeue();
                    Message tip = GetTip();
                    _linkedList.AddLast(tip);
                    tip.SetMessage(message);
                }
            }
            //显示
            LinkedListNode<Message> curnode = _linkedList.First;
            while (curnode != null)
            {
                LinkedListNode<Message> nextNode = curnode.Next;
                //做相应的动画
                if (curnode.Value.Tween(Time.deltaTime, out float processing))
                {
                    float value = _curve.Evaluate(processing);
                    curnode.Value.CanvasGroup.alpha = _alphaCurve.Evaluate(processing);
                    curnode.Value.localPos = Vector3.Lerp(_down.anchoredPosition, _up.anchoredPosition, value);
                }
                else
                {
                    //移除当前的节点
                    _linkedList.Remove(curnode);
                    ReleaseTip(curnode.Value);
                }
                curnode = nextNode;
            }
        }

        public void ClearWaitQueue()
        {
            _waitQueue.Clear();
        }

        private void ReleaseTip(Message tip)
        {
            tip.CanvasGroup.alpha = 0;
            tip.SetActive(false);
            _rycycleTipQueue.Enqueue(tip);
        }

        private Message GetTip()
        {
            Message tip = null;
            if (_rycycleTipQueue.Count == 0)
            {
                tip = (Message)Instantiate((Object)tipPrefabs, _root);
            }
            else
            {
                tip = _rycycleTipQueue.Dequeue();
            }
            tip.ResetMessage();
            tip.CanvasGroup.alpha = 1;
            tip.SetActive(true);
            return tip;
        }
    }
}