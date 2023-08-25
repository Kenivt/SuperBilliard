using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class FriendRoomUIForm : UILogicBase
    {
        [SerializeField] private Button _readyBtn;
        [SerializeField] private Text _readyBtnText;
        [SerializeField] private Button _backMainMenuBtn;
        [SerializeField] private InviteFirendScroll _inviteScroll;
        [SerializeField] private PlayerRoomStateUIItem[] _playerStates;

        private RoomReadyBtnStatus _roomBtnStatus;

        private FriendRoomDataBundle _dataBundle;
        private FriendDataBundle _friendDataBundle;

        private bool _backingRoom;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);
            GameEntry.Event.Subscribe(UpdatePlayerReadyStateEventArgs.EventId, OnUpdatePlayerReadyState);
            GameEntry.Event.Subscribe(InviteeEnterRoomEventArgs.EventId, OnInviteeEnterRoom);
            GameEntry.Event.Subscribe(PlayerLeaveFriendRoomEventArgs.EventId, OnPlayerLeaveRoom);

            GameEntry.Event.Subscribe(UpdateFriendStateEventArgs.EventId, OnUpdateFriendState);

            SetReadyBtnText(RoomReadyBtnStatus.Ready);

            _dataBundle = GameEntry.DataBundle.GetData<FriendRoomDataBundle>();

            //设置button的点击事件
            _readyBtn.onClick.AddListener(() =>
            {
                bool isReady = !_dataBundle.IsReady;
                GameEntry.Client.SendReadyGamePacket(isReady);
            });

            //是否返回房间
            _backingRoom = false;
            _backMainMenuBtn.onClick.AddListener(() =>
            {
                if (_backingRoom == false)
                    GameEntry.Client.SendLeaveGameRoom();
            });

            //初始化好友信息
            _inviteScroll.Initlize(GameEntry.DataBundle.GetData<FriendDataBundle>().FriendList);

            //更新玩家信息
            //设置玩家的信息
            for (int i = 0; i < _dataBundle.playerDatas.Length; i++)
            {
                PlayerData playerData = _dataBundle.playerDatas[i];
                _playerStates[i].Reset();

                if (playerData != null)
                {
                    _playerStates[i].DisplayPlayerImage(playerData.userName, playerData.nickName,
                        playerData.body, playerData.hair, playerData.face, playerData.kit);
                    PlayerRoomStatus status = playerData.isReady ? PlayerRoomStatus.Ready : PlayerRoomStatus.NotReady;
                    _playerStates[i].SetState(status);
                }
            }
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            _readyBtn.onClick.RemoveAllListeners();
            _backMainMenuBtn.onClick.RemoveAllListeners();

            for (int i = 0; i < _playerStates.Length; i++)
            {
                _playerStates[i].Reset();
            }
            GameEntry.Event.Unsubscribe(UpdateFriendStateEventArgs.EventId, OnUpdateFriendState);

            GameEntry.Event.Unsubscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);
            GameEntry.Event.Unsubscribe(UpdatePlayerReadyStateEventArgs.EventId, OnUpdatePlayerReadyState);
            GameEntry.Event.Unsubscribe(InviteeEnterRoomEventArgs.EventId, OnInviteeEnterRoom);
            GameEntry.Event.Unsubscribe(PlayerLeaveFriendRoomEventArgs.EventId, OnPlayerLeaveRoom);
        }

        private void OnPlayerLeaveRoom(object sender, GameEventArgs e)
        {
            PlayerLeaveFriendRoomEventArgs args = (PlayerLeaveFriendRoomEventArgs)e;

            //好友离开房间
            for (int i = 0; i < _playerStates.Length; i++)
            {
                PlayerRoomStateUIItem ui = _playerStates[i];

                if (ui.UserName == args.UserName)
                {
                    ui.Reset();
                    break;
                }
            }

            //如果是自己离开房间
            if (args.UserName == GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName)
            {
                Log.Info("自己离开房间");
                _dataBundle.Reset();
                _backingRoom = true;
                Close();
            }
        }

        private void OnInviteeEnterRoom(object sender, GameEventArgs e)
        {
            InviteeEnterRoomEventArgs args = (InviteeEnterRoomEventArgs)e;
            //查看是否有重复
            for (int i = 0; i < _playerStates.Length; i++)
            {
                if (_playerStates[i] != null && _playerStates[i].UserName == args.playerData.userName)
                {
                    Log.Warning("重复的玩家进入房间...");
                    return;
                }
            }

            for (int i = 0; i < _playerStates.Length; i++)
            {
                PlayerRoomStateUIItem ui = _playerStates[i];

                if (ui.HadDisplay == false)
                {
                    ui.DisplayPlayerImage(args.playerData.userName, args.playerData.nickName, args.playerData.body, args.playerData.hair, args.playerData.face, args.playerData.kit);
                    break;
                }
            }
        }

        private void OnUpdateFriendState(object sender, GameEventArgs e)
        {
            _inviteScroll.RefrashViewRangeData();
        }

        private void OnUpdateFriendList(object sender, GameEventArgs e)
        {
            UpdateFriendListEventArgs args = (UpdateFriendListEventArgs)e;

            if (_inviteScroll.Datas != args.FriendList)
            {
                _inviteScroll.Initlize(args.FriendList);
            }
            else
            {
                _inviteScroll.RefrashViewRangeData();
            }
        }

        private void OnUpdatePlayerReadyState(object sender, GameEventArgs e)
        {
            UpdatePlayerReadyStateEventArgs args = (UpdatePlayerReadyStateEventArgs)e;

            //如果是自己,改变按钮的状态
            string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            if (args.UserName == username)
            {
                RoomReadyBtnStatus roomReadyBtnStatus = args.IsReady ? RoomReadyBtnStatus.Ready : RoomReadyBtnStatus.Cancel;
                SetReadyBtnText(roomReadyBtnStatus);
            }

            for (int i = 0; i < _playerStates.Length; i++)
            {
                PlayerRoomStateUIItem ui = _playerStates[i];

                if (ui.UserName == args.UserName)
                {
                    if (args.IsReady)
                    {
                        ui.SetState(PlayerRoomStatus.Ready);
                    }
                    else
                    {
                        ui.SetState(PlayerRoomStatus.NotReady);
                    }
                    return;
                }
            }
            Log.Error("没有找到对应的玩家...");
        }

        private void SetReadyBtnText(RoomReadyBtnStatus status)
        {
            _roomBtnStatus = status;
            string key = ConvertUtility.EnumToLoaclizationKey(_roomBtnStatus);
            string value = GameEntry.Localization.GetString(key);
            _readyBtnText.text = value;
        }
    }
}