using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SuperBilliard
{
    public class DownList : MonoBehaviour
    {
        [SerializeField] private Button _itemTemplete;
        [SerializeField] protected Transform content;
        [SerializeField] protected Transform view;
        [SerializeField] private Button _controlBtn;
        [SerializeField] private ItemMessage[] _itemMessages;

        private Button[] _itemButtons;
        private Text _controlBtnText;
        public int LastIndex { get; private set; }
        public int CurIndex { get; private set; }

        protected virtual void Awake()
        {
            _itemButtons = new Button[_itemMessages.Length];
            _controlBtnText = _controlBtn.GetComponentInChildren<Text>();
            for (int i = 0; i < _itemMessages.Length; i++)
            {
                _itemButtons[i] = Instantiate(_itemTemplete, content);
                _itemButtons[i].gameObject.SetActive(true);
                _itemButtons[i].GetComponentInChildren<Text>().text = _itemMessages[i].content;
                _itemButtons[i].GetComponentInChildren<Image>().sprite = _itemMessages[i].sprite;
            }
            if (_itemButtons.Length > 0)
            {
                _controlBtnText.text = _itemMessages[0].content;
            }
            view.gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            for (int i = 0; i < _itemMessages.Length; i++)
            {
                int index = i;
                ItemMessage itemMessage = _itemMessages[index];
                Button button = _itemButtons[index];
                button.onClick.AddListener(() =>
                {
                    LastIndex = CurIndex;
                    CurIndex = index;
                    view.gameObject.SetActive(false);
                    _controlBtnText.text = itemMessage.content;
                });
                if (itemMessage.action != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        itemMessage.action.Invoke(itemMessage.content);
                    });
                }
            }
            _controlBtn.onClick.AddListener(OnBtnClick);
        }

        protected virtual void OnDisable()
        {
            for (int i = 0; i < _itemMessages.Length; i++)
            {
                _itemButtons[i].onClick.RemoveAllListeners();
            }
            _controlBtn.onClick.RemoveListener(OnBtnClick);
        }

        public Button GetitemButton(int index)
        {
            if (index < 0 || index >= _itemButtons.Length)
            {
                return null;
            }
            return _itemButtons[index];
        }

        public Button[] GetAllItemButton()
        {
            return _itemButtons;
        }

        public Button GetCurButton()
        {
            return GetitemButton(CurIndex);
        }

        public Button GetLastButton()
        {
            return GetitemButton(LastIndex);
        }

        private void OnBtnClick()
        {
            view.gameObject.SetActive(!view.gameObject.activeSelf);
        }

        [System.Serializable]
        public class ItemMessage
        {
            public string content;
            public Sprite sprite;
            public UnityEvent<string> action;
        }
    }
}
