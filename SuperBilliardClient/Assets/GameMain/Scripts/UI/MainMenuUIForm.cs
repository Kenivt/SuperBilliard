using Knivt.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace SuperBilliard
{
    public class MainMenuUIForm : UILogicBase
    {
        private Button _settingBtn;
        private Button _soundBtn;
        private Button _firendBtn;

        [SerializeField] private Image _hair;
        [SerializeField] private Image _face;
        [SerializeField] private Image _kit;
        [SerializeField] private Image _body;
        [SerializeField] private Button _playerImageBtn;

        [SerializeField] private Text _nickNametext;
        [SerializeField] private Text _levelText;
        private PlayerImageData _data;

        private bool _mute = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _settingBtn = transform.GetComponentFromOffspring<Button>("SettingBtn");
            _soundBtn = transform.GetComponentFromOffspring<Button>("SoundBtn");
            _firendBtn = transform.GetComponentFromOffspring<Button>("FriendBtn");
            _data = GameEntry.DataBundle.GetData<PlayerDataBundle>().PlayerImage;
            _playerImageBtn.onClick.AddListener(() =>
            {
                GameEntry.UI.OpenUIForm(EnumUIForm.PlayerImageCostomUIForm, _data);
            });

            _settingBtn.onClick.AddListener(OnClickSettingBtn);
            _soundBtn.onClick.AddListener(OnClickSoundBtn);
            _firendBtn.onClick.AddListener(OnClickFirendBtn);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            PlayerDataBundle playerBundle = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            UpdatePlayerImage();
            _levelText.text = playerBundle.Level.ToString();
            _nickNametext.text = playerBundle.NickName;
        }

        private void OnClickSettingBtn()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.GameSettingsUIForm);
        }

        private void OnClickSoundBtn()
        {
            //_mute = !_mute;
            //foreach (var item in GameEntry.Sound.GetAllSoundGroups())
            //{
            //    item.Mute = _mute;
            //}
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdatePlayerImage();
            PlayerDataBundle playerBundle = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            _levelText.text = playerBundle.Level.ToString();
            _nickNametext.text = playerBundle.NickName;
        }

        private void OnClickFirendBtn()
        {
            GameEntry.UI.OpenUIForm(EnumUIForm.FriendUIForm);
        }

        private void UpdatePlayerImage()
        {
            //初始化玩家形象
            _body.sprite = GameEntry.ResourceCache.GetBodySprite(_data.BodyId);
            _face.sprite = GameEntry.ResourceCache.GetFaceSprite(_data.FacaId);
            _kit.sprite = GameEntry.ResourceCache.GetKitSprite(_data.KitId);
            _hair.sprite = GameEntry.ResourceCache.GetHairSprite(_data.HairId);
        }
    }
}