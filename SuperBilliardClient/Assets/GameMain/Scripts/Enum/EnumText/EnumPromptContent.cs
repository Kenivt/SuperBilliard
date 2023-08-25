using UnityEngine;
using UnityGameFramework;

namespace SuperBilliard
{
    [LoaclizationEnumText]
    public enum EnumPromptContent
    {
        [LoaclizationEnumAdditional("The language configuration is saved successfully." +
            " If you need to restart the game for application Settings, do you want to restart it?")]
        SaveLanguageSetting,
        [LoaclizationEnumAdditional("Do you want to save the game Settings?")]
        SaveSettings,
    }
}