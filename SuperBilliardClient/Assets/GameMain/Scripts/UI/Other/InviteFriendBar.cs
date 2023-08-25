using GameMessage;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class InviteFriendBar : MonoBehaviour
    {
        private FriendMessage _data;

        [SerializeField] private Image face;
        [SerializeField] private Image hair;
        [SerializeField] private Image body;
        [SerializeField] private Image kit;

        [SerializeField] private Text nickName;

        [SerializeField] private Text _descriotion;

        [SerializeField] private Text _stateText;

        private Button _button;
        public void Display(FriendMessage data)
        {
            _data = data;

            face.sprite = _data.face;
            hair.sprite = _data.hair;
            body.sprite = _data.body;
            kit.sprite = _data.kit;
            nickName.text = _data.NickName;
            //设置当前的状态
            string key = ConvertUtility.EnumToLoaclizationKey(_data.status);
            string value = GameEntry.Localization.GetString(key);
            _stateText.text = value;
            _descriotion.text = _data.description;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnInviteFriend);
        }

        private void OnInviteFriend()
        {
            //只有对方是空闲状态才能邀请
            if (_data.status == PlayerStatus.Idle)
            {
                CSInviteFriendBattle packet = new CSInviteFriendBattle();

                var dataBundle = GameEntry.DataBundle.GetData<FriendRoomDataBundle>();
                packet.OwnUserName = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
                packet.InviteeUserName = _data.UserName;

                packet.GameType = dataBundle.GameType;
                packet.RoomId = dataBundle.RoomId;

                GameEntry.Client.Send(packet);
            }
            else
            {
                GameEntry.Event.Fire(this, ShowMessageEventArgs.Create("对方正在忙......"));
            }
        }
    }
}