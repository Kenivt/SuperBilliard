using UnityEngine;
using System.Collections;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public static class UIExtension
    {
        private static int? downloadFormId = null;
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }
            canvasGroup.alpha = alpha;
        }
        public static void CloseUIForm(this UIComponent uiComponent, UILogicBase uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }
        public static void CloseUIForm(this UIComponent uiComponent, UIForm uiForm)
        {
            if (uiForm == null)
            {
                Log.Warning("UI form is invalid.");
                return;
            }

            uiComponent.CloseUIForm(uiForm.SerialId);
        }
        public static int? OpenUIForm(this UIComponent uiComponent, EnumUIForm uiFormId, object userData = null)
        {
            return OpenUIForm(uiComponent, (int)uiFormId, userData);
        }
        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            UIData uiData = null;
            if (GameEntry.DataBundle.GetData<UIDataBundle>().TryGetData(uiFormId, out uiData) == false)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = uiData.DrAssetPath.AssetPath;
            //如果没有找到对应的UIGroup就创建一个
            if (GameEntry.UI.HasUIGroup(uiData.UIGroup.Name) == false)
            {
                GameEntry.UI.AddUIGroup(uiData.UIGroup.Name, uiData.UIGroup.Depth);
            }
            if (!uiData.DrUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(assetName, uiData.UIGroup.Name, uiData.DrUIForm.PauseCoveredUIForm, userData);
        }
    }
}