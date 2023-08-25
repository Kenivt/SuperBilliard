using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SuperBilliard
{
    public class FriendUIForm : UILogicBase
    {
        [SerializeField] private SelectToggle _firendTog;
        [SerializeField] private SelectToggle _addFirendTog;
        [SerializeField] private Button _backMainMenu;
        [SerializeField] private Button _frientRequest;

        [Header("CANVAS")]
        [SerializeField] private CanvasGroup _firendCanvas;
        [SerializeField] private CanvasGroup _addFirendCanvas;

        [Header("SCROLL")]
        [SerializeField] private FirendMessageScroll _friendScroll;
        [SerializeField] private AddFirendMessageScroll _addFriendScroll;

        [Header("SEARCH PLAYER")]
        [SerializeField] private Button _searchBtn;
        [SerializeField] private InputField _searchPlayerInput;

        public const string pattern = @"^[0-9a-zA-Z]{12}$";

        private bool _searchIng;
        private string _searchUserName;

        private List<FriendMessage> _firendMessages;

        private List<AddFriendMessageData> _searchPlayerData = new List<AddFriendMessageData>();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            _firendTog.onClick.AddListener(OpenFirendCanvas);
            _addFirendTog.onClick.AddListener(OpenAddFirendCanvas);
            _backMainMenu.onClick.AddListener(Close);

            _frientRequest.onClick.AddListener(OnClickFirendRequestBtn);
            _searchBtn.onClick.AddListener(OnClickSearchBtn);
        }

        private void OnClickSearchBtn()
        {
            InputField inputField = _searchPlayerInput;

            if (Regex.IsMatch(inputField.text, pattern) && !_searchIng)
            {
                _searchUserName = inputField.text;
                _searchIng = true;
                GameEntry.Client.SendGetPlayerMessageRequest(inputField.text);
            }
            else
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create("请输入正确的玩家信息..."));
            }
        }

        private void OnClickFirendRequestBtn()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.FriendRequestUIForm);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OpenFirendCanvas();
            GameEntry.Event.Subscribe(RecievePlayerMessageEventArgs.EventId, OnRecievePlayerMessage);
            GameEntry.Event.Subscribe(S2CFriendHandlerResultEventArgs.EventId, OnRequestCallback);
            GameEntry.Event.Subscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);

            //获取好友列表
            var friendDataBundle = GameEntry.DataBundle.GetData<FriendDataBundle>();
            _friendScroll.Initlize(friendDataBundle.FriendList);
        }

        private void OnRequestCallback(object sender, GameEventArgs e)
        {
            S2CFriendHandlerResultEventArgs args = (S2CFriendHandlerResultEventArgs)e;
            if (args.MesssageType == S2CFriendMessageType.FriendRequest)
            {
                string username = args.UserData as string;
                if (username != null)
                {
                    int index = -1;
                    for (int i = 0; i < _searchPlayerData.Count; i++)
                    {
                        if (_searchPlayerData[i].userName == username)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == -1)
                    {
                        return;
                    }
                    if (args.Result == GameMessage.FriendHandleResult.Success)
                    {
                        _searchPlayerData.RemoveAt(index);
                        _addFriendScroll.Refrash();
                    }
                    else
                    {
                        //更换数据
                        //因为是结构体所以不能直接修改
                        var data = _searchPlayerData[index];
                        data.handled = false;
                        _searchPlayerData[index] = data;

                        _addFriendScroll.Refrash();
                    }
                }

            }
        }

        private void OnUpdateFriendList(object sender, GameEventArgs e)
        {
            UpdateFriendListEventArgs args = (UpdateFriendListEventArgs)e;
            _firendMessages = args.FriendList;
            _friendScroll.Initlize(_firendMessages);
        }

        private void OnRecievePlayerMessage(object sender, GameEventArgs e)
        {
            RecievePlayerMessageEventArgs ne = (RecievePlayerMessageEventArgs)e;
            if (ne.UserName == _searchUserName)
            {
                _searchIng = false;
                _searchUserName = string.Empty;
                _searchPlayerData.Clear();
                _searchPlayerData.Add(new AddFriendMessageData()
                {
                    nickName = ne.NickName,
                    userName = ne.UserName,
                    description = ne.Description,
                    hair = GameEntry.ResourceCache.GetHairSprite(ne.HairID),
                    face = GameEntry.ResourceCache.GetFaceSprite(ne.FaceID),
                    body = GameEntry.ResourceCache.GetBodySprite(ne.BodyID),
                    kit = GameEntry.ResourceCache.GetKitSprite(ne.KitID),
                });
                _addFriendScroll.Initlize(_searchPlayerData);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(RecievePlayerMessageEventArgs.EventId, OnRecievePlayerMessage);
            GameEntry.Event.Unsubscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);
            GameEntry.Event.Unsubscribe(S2CFriendHandlerResultEventArgs.EventId, OnRequestCallback);
        }

        private void OpenFirendCanvas()
        {
            _firendCanvas.alpha = 1;
            _firendCanvas.blocksRaycasts = true;
            _addFirendCanvas.alpha = 0;
            _addFirendCanvas.blocksRaycasts = false;
            _firendTog.IsSelecting = true;
            _addFirendTog.IsSelecting = false;
        }

        private void OpenAddFirendCanvas()
        {
            _firendCanvas.alpha = 0;
            _firendCanvas.blocksRaycasts = false;
            _addFirendCanvas.alpha = 1;
            _addFirendCanvas.blocksRaycasts = true;
            _firendTog.IsSelecting = false;
            _addFirendTog.IsSelecting = true;
        }
    }
}
