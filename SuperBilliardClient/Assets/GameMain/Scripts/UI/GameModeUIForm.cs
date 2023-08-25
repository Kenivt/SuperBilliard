using GameMessage;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class GameModeUIForm : UILogicBase
    {
        [Header("MATCH MODE")]
        [SerializeField] private Button _fancyBilliardsBtn;
        [SerializeField] private Button _snookerBilliardsBtn;
        [SerializeField] private Button _friendModeBtn;
        [SerializeField] private CanvasGroup _matchModeCanvasGroup;
        [Header("FIREND MODE")]
        [SerializeField] private Button _friendFancyBtn;
        [SerializeField] private Button _friendSnookerdBtn;
        [SerializeField] private Button _backMatchMode;
        [SerializeField] private CanvasGroup _firendModeCanvasGroup;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _fancyBilliardsBtn.onClick.AddListener(() =>
            {
                GameEntry.Sound.PlayMusic(EnumSound.Click_Soft_00);
                FancyBIlliard();
            });
            _snookerBilliardsBtn.onClick.AddListener(() =>
            {
                GameEntry.Sound.PlayMusic(EnumSound.Click_Soft_00);
                SnokkerMatch();
            });
            _friendModeBtn.onClick.AddListener(() =>
            {
                GameEntry.Sound.PlayMusic(EnumSound.Click_Soft_00);
                _matchModeCanvasGroup.alpha = 0;
                _firendModeCanvasGroup.alpha = 1;
                _firendModeCanvasGroup.blocksRaycasts = true;
                _matchModeCanvasGroup.blocksRaycasts = false;
            });
            _backMatchMode.onClick.AddListener(() =>
            {
                GameEntry.Sound.PlayMusic(EnumSound.Click_Soft_00);
                _matchModeCanvasGroup.alpha = 1;
                _firendModeCanvasGroup.alpha = 0;
                _matchModeCanvasGroup.blocksRaycasts = true;
                _firendModeCanvasGroup.blocksRaycasts = false;
            });
            _friendFancyBtn.onClick.AddListener(() =>
            {
                GameEntry.Sound.PlayMusic(EnumSound.Click_Soft_00);
                FirendFancyBilliard();
            });
            _friendSnookerdBtn.onClick.AddListener(() =>
            {
                GameEntry.Sound.PlayMusic(EnumSound.Click_Soft_00);
                FriendSnokker();
            });
        }

        private void FancyBIlliard()
        {
            //发送匹配请求
            string userName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            GameEntry.Client.Send(CSJoinGame.Create(userName, GameType.FancyMatch));

            //显示等待界面
            GameEntry.UI.OpenUIForm(EnumUIForm.WaitingUIForm);

            //设置游戏数据
            PlayerDataBundle playerDataBunlde = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            playerDataBunlde.curPlayerGameType = GameType.FancyMatch.ToEnumBattle();
        }

        private void FirendFancyBilliard()
        {
            //发送创建房间的请求
            string userName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            //发送创建房间的请求
            GameEntry.Client.SendCreateRoomPacket(userName, GameType.FancyFriend);
            //之后会收到服务器的回复，然后再打开房间界面
        }

        private void FriendSnokker()
        {
            //发送创建房间的请求
            string userName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            //发送创建房间的请求
            GameEntry.Client.SendCreateRoomPacket(userName, GameType.SnookerFriend);
            //之后会收到服务器的回复，然后再打开房间界面
        }
        private void SnokkerMatch()
        {
            //发送匹配请求
            string userName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            GameEntry.Client.Send(CSJoinGame.Create(userName, GameType.SnookerMatch));
            //显示等待界面
            GameEntry.UI.OpenUIForm(EnumUIForm.WaitingUIForm);

            //设置游戏数据
            PlayerDataBundle playerDataBunlde = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            playerDataBunlde.curPlayerGameType = GameType.SnookerMatch.ToEnumBattle();
        }
    }
}