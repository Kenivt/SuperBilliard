using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class FriendRequestUIForm : UILogicBase
    {
        [SerializeField] private Button _backBtn;

        [SerializeField] private FriendRequestScroll _friendRequestScroll;
        private List<FriendRequestData> _friendRequestDatas;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _backBtn.onClick.AddListener(Close);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            //更新好友请求列表
            FriendDataBundle friendDataBundle = GameEntry.DataBundle.GetData<FriendDataBundle>();
            _friendRequestDatas = friendDataBundle.FriendRequestList;
            friendDataBundle.UpdateFriendRequestData();

            GameEntry.Event.Subscribe(UpdateFriendRequestListEventArgs.EventId, OnRecieveFriendRequestList);
            GameEntry.Event.Subscribe(S2CFriendHandlerResultEventArgs.EventId, ProcessingCallback);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(UpdateFriendRequestListEventArgs.EventId, OnRecieveFriendRequestList);
            GameEntry.Event.Unsubscribe(S2CFriendHandlerResultEventArgs.EventId, ProcessingCallback);
        }

        private void ProcessingCallback(object sender, GameEventArgs e)
        {
            S2CFriendHandlerResultEventArgs args = (S2CFriendHandlerResultEventArgs)e;
            if (args.MesssageType != S2CFriendMessageType.ProcessingFriendRequest)
            {
                return;
            }
            if (args.Result == GameMessage.FriendHandleResult.Success)
            {
                GameEntry.Event.Fire(0, ShowMessageEventArgs.Create("成功,处理好友请求..."));
                string username = args.UserData as string;
                if (username != null)
                {
                    int deleteIndex = -1;
                    for (int i = 0; i < _friendRequestDatas.Count; i++)
                    {
                        if (_friendRequestDatas[i].userName == username)
                        {
                            deleteIndex = i;
                            break;
                        }
                    }
                    if (deleteIndex != -1)
                    {
                        _friendRequestDatas.RemoveAt(deleteIndex);
                        _friendRequestScroll.Initlize(_friendRequestDatas);
                    }
                    else
                    {
                        Log.Error("处理好友请求失败,事件传入的用户名不在列表中...");
                    }
                }
                else
                {
                    Log.Error("处理好友请求失败,事件没有没有传入对应用户名...");
                }
            }
            else
            {
                GameEntry.Event.Fire(0, ShowMessageEventArgs.Create("错误,显示信息失败..."));
            }
        }

        private void OnRecieveFriendRequestList(object sender, GameEventArgs e)
        {
            UpdateFriendRequestListEventArgs ne = (UpdateFriendRequestListEventArgs)e;
            _friendRequestDatas = ne.FriendRequestList;
            _friendRequestScroll.Initlize(_friendRequestDatas);
        }
    }
}
