using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class PlayerDataUI : MonoBehaviour
    {
        public Text nicknameText;
        public Text levelText;

        public Image _bodyImage;
        public Image _hairImage;
        public Image _faceImage;
        public Image _kitImage;

        public void Display(string nickName, string level, Sprite body, Sprite hair, Sprite face, Sprite kit)
        {
            nicknameText.text = nickName;
            levelText.text = level;
            _bodyImage.sprite = body;
            _hairImage.sprite = hair;
            _faceImage.sprite = face;
            _kitImage.sprite = kit;
        }
    }
}
