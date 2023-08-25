using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class PlayerRoomStateUIItem : MonoBehaviour
    {
        [SerializeField] private Text _stateText;

        [SerializeField] private Image _bodyImage;
        [SerializeField] private Image _hairImage;
        [SerializeField] private Image _faceImage;
        [SerializeField] private Image _kitImage;
        [SerializeField] private Text _nickName;

        public string UserName { get; private set; }

        public PlayerRoomStatus Status { get; set; }

        public bool HadDisplay { get; private set; }

        public void DisplayPlayerImage(string username, string nickName, Sprite body, Sprite hair, Sprite face, Sprite kit)
        {
            gameObject.SetActive(true);
            this.UserName = username;
            _bodyImage.sprite = body;
            _hairImage.sprite = hair;
            _faceImage.sprite = face;
            _kitImage.sprite = kit;
            _nickName.text = nickName;
            HadDisplay = true;
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            _bodyImage.sprite = null;
            _hairImage.sprite = null;
            _faceImage.sprite = null;
            _kitImage.sprite = null;
            _nickName.text = "";
            UserName = "";
            SetState(PlayerRoomStatus.NotReady);
            HadDisplay = false;
        }

        public void SetState(PlayerRoomStatus status)
        {
            Status = status;
            string key = ConvertUtility.EnumToLoaclizationKey(status);

            string value = GameEntry.Localization.GetString(key);

            if (status == PlayerRoomStatus.NotReady)
            {
                _stateText.text = string.Format(Constant.TextTemplates.Ready, value);
            }
            else if (status == PlayerRoomStatus.Ready)
            {
                _stateText.text = string.Format(Constant.TextTemplates.NotReady, value);
            }
        }
    }
}