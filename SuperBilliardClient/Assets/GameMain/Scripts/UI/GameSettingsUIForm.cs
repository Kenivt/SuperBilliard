using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using GameFramework.Localization;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class GameSettingsUIForm : UILogicBase
    {
        [Header("CANVAS GROUP")]

        [SerializeField] private CanvasGroup _soundSettingCanvasGroup;
        [SerializeField] private CanvasGroup _graphicSettingCanvasGroup;
        [SerializeField] private CanvasGroup _otherSettingCanvasGroup;

        [Header("BUTTON")]
        [SerializeField] private SelectToggle _soundSettingBtn;
        [SerializeField] private SelectToggle _graphicSettingBtn;
        [SerializeField] private SelectToggle _gameSettingBtn;
        [SerializeField] private Button _backMainmenuBtn;
        [SerializeField] private Button _exitGameBtn;

        [Header("SLIDER")]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _uiSoundSlider;
        [SerializeField] private Slider _sfxSlider;

        [Header("TOGGLE")]
        [SerializeField] private Toggle _fullScreenToggle;

        [Header("DROPDOWN")]
        [SerializeField] private Dropdown _resolution;
        [SerializeField] private Dropdown _frameRate;
        [SerializeField] private Dropdown _language;

        private List<SelectToggle> _selectBtns;

        private bool _languageSettingChane = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            _soundSettingBtn.BindingCanvasGroup = _soundSettingCanvasGroup;
            _graphicSettingBtn.BindingCanvasGroup = _graphicSettingCanvasGroup;
            _gameSettingBtn.BindingCanvasGroup = _otherSettingCanvasGroup;

            _selectBtns = new List<SelectToggle>
            {
                _soundSettingBtn,
                _graphicSettingBtn,
                _gameSettingBtn
            };


            //添加事件
            _musicSlider.onValueChanged.AddListener((value) =>
            {
                GameEntry.Sound.SetMusicVolume(value);
            });
            _uiSoundSlider.onValueChanged.AddListener((value) =>
            {
                GameEntry.Sound.SetUISoundVolume(value);
            });
            _sfxSlider.onValueChanged.AddListener((value) =>
            {
                GameEntry.Sound.SetSFXVolume(value);
            });
            _fullScreenToggle.onValueChanged.AddListener((flag) =>
            {
                Screen.fullScreen = flag;
                GameEntry.Setting.SetBool(Constant.GameSetting.FullScreen, flag);
                GameEntry.Setting.Save();
            });
            _resolution.onValueChanged.AddListener(SelectResolution);
            _frameRate.onValueChanged.AddListener(SetFrameRate);
            _language.onValueChanged.AddListener(SetLanguage);

            for (int i = 0; i < _selectBtns.Count; i++)
            {
                int index = i;
                _selectBtns[index].onClick.AddListener(() =>
                {
                    for (int j = 0; j < _selectBtns.Count; j++)
                    {
                        if (j != index)
                        {
                            _selectBtns[j].IsSelecting = false;
                        }
                    }
                    _selectBtns[index].IsSelecting = true;
                });
            }
            _backMainmenuBtn.onClick.AddListener(BackMainMenu);
            _exitGameBtn.onClick.AddListener(ExitGame);
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _languageSettingChane = false;
            //初始化对应的数据
            _selectBtns[0].IsSelecting = true;
            for (int j = 1; j < _selectBtns.Count; j++)
            {
                _selectBtns[j].IsSelecting = false;
            }

            //设置对应的初始数据
            _musicSlider.value = GameEntry.Setting.GetFloat(GameFramework.Utility.Text.Format(Constant.GameSetting.SoundGroupVolume, "Music"), 1);
            _uiSoundSlider.value = GameEntry.Setting.GetFloat(GameFramework.Utility.Text.Format(Constant.GameSetting.SoundGroupVolume, "SFX/UI"), 1);
            _sfxSlider.value = GameEntry.Setting.GetFloat(GameFramework.Utility.Text.Format(Constant.GameSetting.SoundGroupVolume, "SFX"), 1);
            _fullScreenToggle.isOn = GameEntry.Setting.GetBool(Constant.GameSetting.FullScreen, true);

            _language.value = GetLanguageIndex();
            _resolution.value = GetResolutionIndex();
            _frameRate.value = GetFrameRateIndex();
            _musicSlider.value = GameEntry.Sound.GetMusicVoluem();
            _uiSoundSlider.value = GameEntry.Sound.GetUISoundVolume();
            _sfxSlider.value = GameEntry.Sound.GetSFXVolume();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        private void BackMainMenu()
        {
            Close(true);
        }

        private void ExitGame()
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }
            else
            {
                UnityGameFramework.Runtime.GameEntry.Shutdown(UnityGameFramework.Runtime.ShutdownType.Quit);
            }
        }

        public void SelectResolution(int id)
        {
            int width = 1920;
            int height = 1080;
            if (id == 0)
            {
                width = 1920;
                height = 1080;
            }
            else if (id == 1)
            {
                width = 1280;
                height = 720;
            }
            if (width != Screen.width || height != Screen.height)
            {
                Screen.SetResolution(width, height, Screen.fullScreen);
                GameEntry.Setting.SetInt(Constant.GameSetting.ScreenWidth, width);
                GameEntry.Setting.SetInt(Constant.GameSetting.ScreenHeight, height);
                GameEntry.Setting.Save();
            }
        }
        public void SetFrameRate(int id)
        {
            int frameRate = 60;
            if (id == 0)
            {
                frameRate = 60;
            }
            else if (id == 1)
            {
                frameRate = 90;
            }

            if (frameRate != Application.targetFrameRate)
            {
                Application.targetFrameRate = frameRate;
                GameEntry.Setting.SetInt(Constant.GameSetting.FrameRate, frameRate);
                GameEntry.Setting.Save();
            }
        }
        public void SetLanguage(int id)
        {
            Language language = GameEntry.Localization.Language;
            if (id == 0)
            {
                language = Language.ChineseSimplified;
            }
            else if (id == 1)
            {
                language = Language.ChineseTraditional;
            }
            else if (id == 2)
            {
                language = Language.English;
            }
            else
            {
                Log.Error("Language is not exist....id is {0}", id);
            }

            if (language == GameEntry.Localization.Language)
            {
                return;
            }
            GameEntry.Setting.SetInt(Constant.GameSetting.Language, (int)language);
            GameEntry.Setting.Save();
            string content = GameEntry.Localization.GetString(EnumPromptContent.SaveLanguageSetting);
            GameEntry.UI.OpenUIForm(EnumUIForm.PromptUIForm,
                PromptMessage.Create(() =>
                {
                    UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
                }, content));
        }

        public int GetLanguageIndex()
        {
            Language language = (Language)GameEntry.Localization.Language;
            if (language == Language.English)
            {
                return 2;
            }
            else if (language == Language.ChineseSimplified)
            {
                return 0;
            }
            else if (language == Language.ChineseTraditional)
            {
                return 1;
            }
            else
            {
                Log.Warning("Language is not exist....language is {0}", language);
                GameEntry.Setting.SetInt(Constant.GameSetting.Language, (int)Language.English);
                GameEntry.Setting.Save();
                return 2;
            }
        }
        private int GetFrameRateIndex()
        {
            int frameRate = Application.targetFrameRate;
            if (frameRate == 60)
            {
                return 0;
            }
            else if (frameRate == 90)
            {
                return 1;
            }
            else
            {
                Log.Warning("FrameRate is not exist....frameRate is {0}", frameRate);
                Application.targetFrameRate = 60;
                GameEntry.Setting.SetInt(Constant.GameSetting.FrameRate, 60);
                GameEntry.Setting.Save();
                return 0;
            }
        }

        private int GetResolutionIndex()
        {
            int width = Screen.width;
            int height = Screen.height;
            if (width == 1920 && height == 1080)
            {
                return 0;
            }
            else if (width == 1280 && height == 720)
            {
                return 1;
            }
            else
            {
                Log.Warning("Resolution is not exist....width is {0},height is {1}", width, height);
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                GameEntry.Setting.SetInt(Constant.GameSetting.ScreenWidth, 1920);
                GameEntry.Setting.SetInt(Constant.GameSetting.ScreenHeight, 1080);
                GameEntry.Setting.Save();
                return 0;
            }
        }
    }
}