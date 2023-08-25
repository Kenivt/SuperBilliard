using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class SnokkerPlayerDataUIItem : MonoBehaviour
    {
        public Image Hair;
        public Image Body;
        public Image Face;
        public Image Kit;
        public Text Score;
        public Text NickName;

        public Image ActiveBilliard;

        public void DisplayMessage(Sprite hair, Sprite body,
            Sprite kit, Sprite face, string nickName)
        {
            Hair.sprite = hair;
            Body.sprite = body;
            Face.sprite = face;
            Kit.sprite = kit;
            NickName.text = nickName;
        }

        public void UpdateScore(int socre)
        {
            Score.text = socre.ToString();
        }

        public void UpdateActiveBilliard(Sprite sprite)
        {
            if (sprite == null)
            {
                ActiveBilliard.gameObject.SetActive(false);
                return;
            }
            ActiveBilliard.gameObject.SetActive(true);
            ActiveBilliard.sprite = sprite;
        }
    }
}
