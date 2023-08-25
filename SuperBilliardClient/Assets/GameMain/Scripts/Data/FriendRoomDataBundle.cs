using UnityEngine;
using GameMessage;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class PlayerData : IReference
    {
        public string userName;
        public string nickName;
        public string description;
        public int level;
        public Sprite face;
        public Sprite hair;
        public Sprite body;
        public Sprite kit;

        public bool isReady;

        public void Clear()
        {
            userName = "";
            nickName = "";
            description = "";
            level = 0;
            face = null;
            hair = null;
            body = null;
            kit = null;
            isReady = false;
        }

        public void PasteFromFriendMessage(FriendMessage friendMessage)
        {
            userName = friendMessage.UserName;
            nickName = friendMessage.NickName;
            description = friendMessage.description;
            face = friendMessage.face;
            hair = friendMessage.hair;
            body = friendMessage.body;
            kit = friendMessage.kit;
        }
        public static PlayerData Create()
        {
            PlayerData playerData = ReferencePool.Acquire<PlayerData>();
            return playerData;
        }
    }

    public class FriendRoomDataBundle : DataBundleBase
    {
        public GameType GameType { get; set; }

        public int RoomId { get; set; }

        public bool IsReady
        {
            get
            {
                string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
                for (int i = 0; i < playerDatas.Length; i++)
                {
                    if (playerDatas[i] != null && playerDatas[i].userName == username)
                    {
                        return playerDatas[i].isReady;
                    }
                }
                Log.Error("自己的数据为空");
                return false;
            }
            set
            {
                string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
                for (int i = 0; i < playerDatas.Length; i++)
                {
                    if (playerDatas[i] != null && playerDatas[i].userName == username)
                    {
                        playerDatas[i].isReady = value;
                        return;
                    }
                }
                Log.Error("自己的数据为空");
            }
        }
        public bool IsAllReady
        {
            get
            {
                for (int i = 0; i < playerDatas.Length; i++)
                {
                    if (playerDatas[i] == null)
                    {
                        return false;
                    }
                    if (!playerDatas[i].isReady)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public PlayerData[] playerDatas { get; private set; } = new PlayerData[2];

        public void AddPlayer(string username, bool isready)
        {
            PlayerDataBundle dataBundle = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            string ownUsername = dataBundle.UserName;
            PlayerData playerData = GetEmptyPlayerData();
            if (playerData == null)
            {
                Log.Error("玩家已经满了,为什么还会有玩家进入呢？.");
                return;
            }

            //如果是自己
            if (ownUsername == username)
            {
                playerData.nickName = dataBundle.NickName;
                playerData.userName = dataBundle.UserName;
                playerData.description = dataBundle.Description;
                playerData.isReady = isready;
                playerData.kit = GameEntry.ResourceCache.GetKitSprite(dataBundle.PlayerImage.KitId);
                playerData.face = GameEntry.ResourceCache.GetFaceSprite(dataBundle.PlayerImage.FacaId);
                playerData.hair = GameEntry.ResourceCache.GetHairSprite(dataBundle.PlayerImage.HairId);
                playerData.body = GameEntry.ResourceCache.GetBodySprite(dataBundle.PlayerImage.BodyId);
            }
            else
            {
                GameEntry.DataBundle.GetData<FriendDataBundle>();
                FriendDataBundle _friendDataBundle = GameEntry.DataBundle.GetData<FriendDataBundle>();
                var data = _friendDataBundle.GetFriendMessage(username);
                if (data == null)
                {
                    Log.Error("好友数据为空");
                    return;
                }
                playerData.PasteFromFriendMessage(data);
                playerData.isReady = isready;
                GameEntry.Event.FireNow(this, InviteeEnterRoomEventArgs.Create(playerData));
            }
        }

        public void SetReadyState(string username, bool isready)
        {
            for (int i = 0; i < playerDatas.Length; i++)
            {
                if (playerDatas[i] != null && playerDatas[i].userName == username)
                {
                    playerDatas[i].isReady = isready;
                    GameEntry.Event.Fire(this, UpdatePlayerReadyStateEventArgs.Create(username, isready));
                    return;
                }
            }

            Log.Error("玩家数据为空");
        }

        public void Reset()
        {
            RoomId = 0;
            GameType = GameType.None;

            for (int i = 0; i < playerDatas.Length; i++)
            {
                if (playerDatas[i] != null)
                {
                    ReferencePool.Release(playerDatas[i]);
                    playerDatas[i] = null;
                }
            }
        }

        public PlayerData GetEmptyPlayerData()
        {
            for (int i = 0; i < 2; i++)
            {
                if (playerDatas[i] == null)
                {
                    playerDatas[i] = PlayerData.Create();
                    return playerDatas[i];
                }
            }
            return null;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }

        public void RemovePlayer(string username)
        {
            for (int i = 0; i < playerDatas.Length; i++)
            {
                if (playerDatas[i] != null && playerDatas[i].userName == username)
                {
                    playerDatas[i] = null;
                    GameEntry.Event.Fire(this, PlayerLeaveFriendRoomEventArgs.Create(username));
                    break;
                }
            }
        }
    }
}