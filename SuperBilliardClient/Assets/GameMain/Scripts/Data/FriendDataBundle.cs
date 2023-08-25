using GameMessage;
using GameFramework.Event;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class FriendDataBundle : DataBundleBase
    {
        public List<FriendMessage> FriendList
        {
            get
            {
                return _friendDataList;
            }
            set
            {
                _friendDataList = value;
            }
        }

        public List<FriendRequestData> FriendRequestList
        {
            get
            {
                return _firendRequestList;
            }
            set
            {
                _firendRequestList = value;
            }
        }

        private List<FriendRequestData> _firendRequestList;
        private List<FriendMessage> _friendDataList;

        private void OnUpdateFriendRequestList(object sender, GameEventArgs e)
        {
            UpdateFriendRequestListEventArgs args = (UpdateFriendRequestListEventArgs)e;
            FriendRequestList = args.FriendRequestList;
        }

        private void OnUpdateFriendList(object sender, GameEventArgs e)
        {
            UpdateFriendListEventArgs args = (UpdateFriendListEventArgs)e;
            FriendList = args.FriendList;
        }

        public bool LoadFriendMessage(List<FriendMessage> friendMessages)
        {
            if (friendMessages == null)
            {
                return false;
            }
            FriendList = friendMessages;
            return true;
        }

        public void UpdateFriendData()
        {
            string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            CSGetFriendList packet = CSGetFriendList.Create(username);

            GameEntry.Client.Send(packet);
        }

        public void UpdateFriendRequestData()
        {
            string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            CSGetFriendRequestList packet = CSGetFriendRequestList.Create(username);
            GameEntry.Client.Send(packet);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GameEntry.Event.Subscribe(UpdateFriendRequestListEventArgs.EventId, OnUpdateFriendRequestList);
            GameEntry.Event.Subscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);
            GameEntry.Event.Subscribe(UpdateFriendStateEventArgs.EventId, OnUpdateFriendStateList);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            GameEntry.Event.Unsubscribe(UpdateFriendRequestListEventArgs.EventId, OnUpdateFriendRequestList);
            GameEntry.Event.Unsubscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);
            GameEntry.Event.Unsubscribe(UpdateFriendStateEventArgs.EventId, OnUpdateFriendStateList);
        }

        public void UpdateFriendState(string username, PlayerStatus status)
        {
            for (int i = 0; i < FriendList.Count; i++)
            {
                if (FriendList[i].UserName.Equals(username))
                {
                    if (status != PlayerStatus.PlayerStausNone)
                    {
                        FriendList[i].isLogin = true;
                    }
                    FriendList[i].status = status;
                    break;
                }
            }
        }

        private void OnUpdateFriendStateList(object sender, GameEventArgs e)
        {
            UpdateFriendStateEventArgs args = (UpdateFriendStateEventArgs)e;

        }
        public FriendMessage? GetFriendMessage(string friendUsername)
        {
            foreach (var friend in FriendList)
            {
                if (friend.UserName == friendUsername)
                {
                    return friend;
                }
            }
            return null;
        }
    }
}