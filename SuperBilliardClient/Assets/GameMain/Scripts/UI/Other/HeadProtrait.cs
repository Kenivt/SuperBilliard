using Knivt.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class HeadProtrait : MonoBehaviour
    {
        private Image _photo;
        private Text _name;


        private void Awake()
        {
            _photo = transform.GetComponentFromOffspring<Image>("Photo");
        }

        public void SetPhoto(Sprite sprite)
        {
            _photo.sprite = sprite;
        }
        public void SetNickName(string name)
        {
            _name.text = name;
        }
    }
}