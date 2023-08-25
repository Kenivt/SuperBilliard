using GameFramework.Fsm;
using GameFramework.Localization;
using SuperBilliard.Constant;
using UnityEngine;

namespace SuperBilliard
{
    public class ProcedureLaunch : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            InitLanguage(); InitBaseSetting();
        }
        protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            ChangeState<ProcedureSplash>(procedureOwner);
        }
        private void InitLanguage()
        {
            if (GameEntry.Base.EditorResourceMode && GameEntry.Base.EditorLanguage != Language.Unspecified)
            {
                // 编辑器资源模式直接使用 Inspector 上设置的语言
                GameEntry.Setting.SetInt(Constant.GameSetting.Language, (int)GameEntry.Base.EditorLanguage);
                GameEntry.Setting.Save();
                return;
            }

            Language language = (Language)GameEntry.Setting.GetInt(Constant.GameSetting.Language, (int)Language.English);
            if (language != Language.English && language != Language.ChineseSimplified && language != Language.ChineseTraditional)
            {
                language = Language.English;
                GameEntry.Localization.Language = Language.English;
                GameEntry.Setting.SetInt(Constant.GameSetting.Language, (int)language);
                GameEntry.Setting.Save();
            }
            else
            {
                GameEntry.Localization.Language = language;
            }
        }
        private void InitBaseSetting()
        {
            int width = GameEntry.Setting.GetInt(Constant.GameSetting.ScreenWidth, 1280);
            int height = GameEntry.Setting.GetInt(Constant.GameSetting.ScreenHeight, 720);
            int frameRate = GameEntry.Setting.GetInt(Constant.GameSetting.FrameRate, 60);
            bool isFullScreen = GameEntry.Setting.GetBool(Constant.GameSetting.FullScreen, false);
            //设置初始数据
            Screen.SetResolution(width, height, isFullScreen);
            Application.targetFrameRate = frameRate;
        }
    }
}
