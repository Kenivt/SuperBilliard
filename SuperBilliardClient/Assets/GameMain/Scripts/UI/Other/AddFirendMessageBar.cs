using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public struct AddFriendMessageData
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

    public class AddFirendMessageBar : MonoBehaviour
    {
        [SerializeField] private Text _nickName;
        [SerializeField] private Text _description;
        [Header("PLAYER IMAGE")]
        public Image hair;
        public Image face;
        public Image body;
        public Image kit;

        public Button _addBtn;

        private AddFriendMessageData _data;

        private void Awake()
        {
            _addBtn.onClick.AddListener(OnClickAddBtn);
        }

        private void OnClickAddBtn()
        {
            if (_data.handled == false)
            {
                _data.handled = true;
                string userName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
                GameEntry.Client.SendRequestFriend(userName, _data.userName);
            }
            else
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create("已经处理过了,不要重复点击"));
            }
        }

        public void Display(AddFriendMessageData data)
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