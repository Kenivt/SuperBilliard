using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class BattleMessageUI : MonoBehaviour
    {
        [SerializeField] private Image[] billiardImages;

        [SerializeField] private Image _hair;
        [SerializeField] private Image _face;
        [SerializeField] private Image _body;
        [SerializeField] private Image _kit;

        private void Start()
        {
            ClearAllBilliard();
        }

        public void DisplayBilliard(int index, Sprite sprite)
        {
            billiardImages[index].gameObject.SetActive(true);
            billiardImages[index].sprite = sprite;
        }
        public void DisplayPlayerImage(Sprite hair, Sprite face, Sprite body, Sprite kit)
        {
            _hair.sprite = hair;
            _face.sprite = face;
            _body.sprite = body;
            _kit.sprite = kit;
        }

        public void ClearAllBilliard()
        {
            for (int i = 0; i < billiardImages.Length; i++)
            {
                billiardImages[i].gameObject.SetActive(false);
                billiardImages[i].sprite = null;
            }
        }
    }
}