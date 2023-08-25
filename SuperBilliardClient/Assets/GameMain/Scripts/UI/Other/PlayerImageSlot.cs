using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SuperBilliard
{

    public class PlayerImageSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Image Image => _playimage;

        [UnityEngine.SerializeField] private Image _playimage;

        public int PlayImageId { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            //发送对应的事件
            GameEntry.Event.Fire(this, ClickPlayerImageSlotEventArgs.Create(PlayImageId, Image.sprite));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }
    }
}
