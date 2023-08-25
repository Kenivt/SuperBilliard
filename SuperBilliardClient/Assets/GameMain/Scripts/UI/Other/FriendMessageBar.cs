using GameMessage;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class FriendMessageBar : MonoBehaviour
    {
        [SerializeField] private Text _nickName;
        [SerializeField] private Text _decription;
        [SerializeField] private Text _loginTag;

        [Header("PLAYER IMAGE")]
        public Image hair;
        public Image face;
        public Image body;
        public Image kit;

        public Button _chatBtn;

        public void Display(FriendMessage message)
        {
            this.hair.sprite = message.hair;
            this.face.sprite = message.face;
            this.body.sprite = message.body;
            this.kit.sprite = message.kit;
            _decription.text = message.description;
            _nickName.text = message.NickName;
            if (message.isLogin)
            {
                _loginTag.text = string.Format(Constant.TextTemplates.Online, "ONLINE");
            }
            else
            {
                _loginTag.text = string.Format(Constant.TextTemplates.Offline, "OFFLINE");
            }
        }
    }

    public class FriendMessage
    {
        public string UserName;
        public string NickName;
        public string description;
        public Sprite hair;
        public Sprite face;
        public Sprite body;
        public Sprite kit;
        public bool isLogin;
        public PlayerStatus status;

        //public void Clear()
        //{
        //    UserName = string.Empty;
        //    NickName = string.Empty;
        //    description = string.Empty;
        //    hair = null;
        //    face = null;
        //    body = null;
        //    kit = null;
        //    isLogin = false;
        //    status = PlayerStatus.PlayerStausNone;
        //}
    }
}
