using System.Collections.Generic;

namespace SuperBilliard
{
    public class PlayerDataItem
    {
        public string UserName { get; set; }
        public string NickName { get; set; }

        public PlayerImageData PlayerImageData { get; set; }

        public string Description { get; set; }

        public int Level { get; set; }
        public PlayerDataItem()
        {
            PlayerImageData = new PlayerImageData();
        }

        public PlayerDataItem(string nickName, string description, PlayerImageData playerImageData)
        {
            NickName = nickName;
            Description = description;
            PlayerImageData = playerImageData;
        }
    }

    public class PlayerDataBundle : DataBundleBase
    {
        public EnumBattle curPlayerGameType;

        public string UserName
        {
            get
            {
                return Own.UserName;
            }
            set
            {
                Own.UserName = value;
            }
        }

        public string NickName
        {
            get
            {
                return Own.NickName;
            }
            set
            {
                Own.NickName = value;
            }
        }

        public string Description
        {
            get
            {
                return Own.Description;
            }
            set
            {
                Own.Description = value;
            }
        }

        public int Level
        {
            get
            {
                return Own.Level;
            }
            set
            {
                Own.Level = value;
            }
        }

        public PlayerImageData PlayerImage
        {
            get
            {
                return Own.PlayerImageData;
            }
        }


        public PlayerDataItem Own { get; set; }

        /// <summary>
        /// 其他的玩家信息
        /// </summary>
        public PlayerDataItem Opponent { get; set; }

        protected override void OnLoad()
        {
            base.OnLoad();
            Opponent = new PlayerDataItem("default", "default", new PlayerImageData());
            //设置信息
            Own = new PlayerDataItem("default", "default", new PlayerImageData());
            Own.PlayerImageData = new PlayerImageData();
        }

        public void SavePlayerMessage()
        {
            GameEntry.Client.SendSavePlayerMessageRequest(UserName, NickName, Description,
                PlayerImage.HairId, PlayerImage.BodyId, PlayerImage.FacaId, PlayerImage.KitId);
        }
    }
}