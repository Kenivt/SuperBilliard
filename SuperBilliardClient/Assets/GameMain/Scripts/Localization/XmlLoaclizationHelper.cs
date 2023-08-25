using System;
using System.Xml.Linq;
using GameFramework.Localization;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class XmlLoaclizationHelper : DefaultLocalizationHelper
    {
        public override bool ParseData(ILocalizationManager localizationManager, string dictionaryString, object userData)
        {
            try
            {
                string currentLanguage = GameEntry.Localization.Language.ToString();
                XDocument doc = XDocument.Parse(dictionaryString);
                var dictionaries = doc.Element("Dictionaries");
                if (dictionaries == null)
                {
                    Log.Warning("Can not parse dictionary string which '{0}' is invalid.", dictionaryString);
                    return false;
                }
                var language = dictionaries.Attribute("Language");
                if (language == null)
                {
                    Log.Warning("Language XElement is invalid.");
                    return false;
                }
                var dictionary = dictionaries.Element("Dictionary");
                if (dictionary == null)
                {
                    Log.Warning("Dictionary xelement is invalid.");
                    return false;
                }
                if (!language.Value.Equals(currentLanguage))
                {
                    Log.Warning("The language parameters are different.{0},{1}", language.Value, currentLanguage);
                    return false;
                }
                var strings = dictionary.Elements("String");
                foreach (var item in strings)
                {
                    var key = item.Attribute("Key");
                    var value = item.Attribute("Value");
                    if (key == null || value == null)
                    {
                        Log.Warning("Key or value is invalid.key = '{0}',value = '{1}'", key, value);
                        return false;
                    }
                    if (!localizationManager.AddRawString(key.Value, value.Value))
                    {
                        Log.Warning("Can not add raw string with key '{0}' which may be duplicate.", dictionaryString);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse dictionary data with exception '{0}'.", exception.ToString());
                return false;
            }
        }
    }
}
