using System;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public static class LocalizationExtension
    {
        public static string GetString<T>(this LocalizationComponent component, T enumText) where T : Enum
        {
            string key = ConvertUtility.EnumToLoaclizationKey(enumText);
            return component.GetString(key);
        }
    }
}