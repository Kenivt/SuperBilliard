using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SuperBilliard.Editor
{
    public static class LoaclizationXmlGenerator
    {
        private static readonly string XmlPath = Application.dataPath + @"/GameMain/Localization/English/Dictionaries";

        private const string language = "English";

        private static string[] loadPaths =
        {
             "Assets/GameMain/UI/UIForm"
        };

        private static string[] generatorLanguages =
        {
            "ChineseSimplified",
            "ChineseTraditional",
        };

        private const string RuntimeAssemblyName = "Assembly-CSharp";

        [UnityEditor.MenuItem("Tools/XML Generate Localization")]
        public static void GenerateLocalizationXml()
        {
            try
            {
                Dictionary<string, string> dic = GetKeyValuePairs();

                if (!Directory.Exists(XmlPath))
                {
                    Directory.CreateDirectory(XmlPath);
                }

                XElement root = new XElement("Dictionaries", new XAttribute("Language", language));
                XElement dictionary = new XElement("Dictionary");
                root.Add(dictionary);
                foreach (var item in dic)
                {
                    dictionary.Add(new XElement("String", new XAttribute("Key", item.Key), new XAttribute("Value", item.Value)));
                }
                XDocument doc = new XDocument(root);
                doc.Save(XmlPath + "/Default.xml");
                AssetDatabase.Refresh();
                Debug.Log("Generate Localization Xml Success!");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [UnityEditor.MenuItem("Tools/XML Generate Location Xml Copy File")]
        public static void GenerateCopyXmlFile()
        {
            Dictionary<string, string> templateDic = GetKeyValuePairs();

            foreach (var language in generatorLanguages)
            {
                string path = Application.dataPath + @"/GameMain/Localization/" + language + "/Dictionaries/Default.xml";
                string copyXmlFilePath = Application.dataPath + @"/GameMain/Localization/" + language + "/Dictionaries";
                Dictionary<string, string> fileDic = null;

                //判断路径是否存在
                if (!Directory.Exists(copyXmlFilePath))
                {
                    Directory.CreateDirectory(copyXmlFilePath);
                }
                //判断文件是否存在
                if (File.Exists(path))
                {
                    fileDic = GetXmlFileDic(path);
                }

                //生成对应的xml文件
                XElement root = new XElement("Dictionaries", new XAttribute("Language", language));
                XElement dictionary = new XElement("Dictionary");
                root.Add(dictionary);
                foreach (var item in templateDic)
                {
                    string value = item.Value;

                    if (fileDic != null && fileDic.TryGetValue(item.Key, out string value1))
                    {
                        value = value1;
                    }

                    dictionary.Add(new XElement("String", new XAttribute("Key", item.Key), new XAttribute("Value", value)));
                }
                XDocument doc = new XDocument(root);
                doc.Save(copyXmlFilePath + "/Default.xml");
                Debug.Log($"Generate Copy {language} Xml File Success!");
            }
        }

        private static Dictionary<string, string> GetKeyValuePairs()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var prefabs = UnityEditor.AssetDatabase.FindAssets("t:Prefab", loadPaths);

            //获取LocalizeText组件中的内容
            foreach (var prefab in prefabs)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(prefab);
                var go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var components = go.GetComponentsInChildren<LocalizeText>(true);
                foreach (var component in components)
                {
                    var keys = component.key;
                    var text = component.GetComponent<UnityEngine.UI.Text>();
                    dic.Add(keys, text.text);
                }
            }

            //从枚举中获取序列化内容
            Assembly assembly = Assembly.Load(RuntimeAssemblyName);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsEnum)
                {
                    Attribute? attribute = type.GetCustomAttribute(typeof(LoaclizationEnumTextAttribute));
                    if (attribute == null)
                    {
                        continue;
                    }
                    FieldInfo[] fields = type.GetFields();

                    for (int i = 1; i < fields.Length; i++)
                    {
                        FieldInfo field = fields[i];
                        string value = field.Name.ToUpper();
                        string key = string.Format(ConvertUtility.EnumLocalizationKeyTemplate, type.Name.ToUpper(), value);

                        Attribute? add = field.GetCustomAttribute(typeof(LoaclizationEnumAdditionalAttribute));
                        if (add != null)
                        {
                            value = (add as LoaclizationEnumAdditionalAttribute).AdditionalText;
                        }
                        dic.Add(key, value);
                    }
                }
            }
            return dic;
        }

        private static Dictionary<string, string> GetXmlFileDic(string path)
        {
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;
            XElement dictionary = root.Element("Dictionary");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in dictionary.Elements())
            {
                var key = item.Attribute("Key").Value;

                var value = item.Attribute("Value").Value;
                if (dic.ContainsKey(key) == false)
                {
                    dic.Add(key, value);
                }
                else
                {
                    Debug.LogError("Key is repeat:" + key);
                }
            }
            return dic;
        }
    }
}