using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public struct FriendRequestData
    {
        public string userName;
        public string nickName;
        public string description;
        public Sprite hair;
        public Sprite face;
        public Sprite body;
        public Sprite kit;
        public bool handled;
    }

    public class FriendRequestBar : MonoBehaviour
    {
        [SerializeField] private Text _nickName;
        [SerializeField] private Text _description;
        [Header("PLAYER IMAGE")]
        public Image hair;
        public Image face;
        public Image body;
        public Image kit;

        public Button _acceptBtn;
        public Button _refuseBtn;

        private FriendRequestData _data;

        private void Awake()
        {
            _acceptBtn.onClick.AddListener(OnClickAcceptBtn);
            _refuseBtn.onClick.AddListener(OnClickRefuseBtn);
        }

        private void OnClickAcceptBtn()
        {
            if (_data.handled == false)
            {
                _data.handled = true;
                string userName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
                GameEntry.Client.SendProcessingFriendRequest(userName, _data.userName, true);
            }
            else
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create("已经处理过了"));
            }
        }

        private void OnClickRefuseBtn()
        {
            if (_data.handled == false)
            {
                _data.handled = true;
                string userName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
                GameEntry.Client.SendProcessingFriendRequest(userName, _data.userName, false);
            }
            else
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create("已经处理过了"));
            }
        }

        public void Display(FriendRequestData data)
        {
            _data = data;
            this.hair.sprite = data.hair;
            this.face.sprite = data.face;
            this.body.sprite = data.body;
            this.kit.sprite = data.kit;
            _nickName.text = data.nickName;
            _description.text = data.description;
        }
    }
}