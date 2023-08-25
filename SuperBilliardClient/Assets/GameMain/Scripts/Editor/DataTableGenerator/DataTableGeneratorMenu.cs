//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
using System.IO;
using UnityEditor;
using UnityEngine;
using GameFramework;
using System.Xml.Linq;

namespace GFLearn.Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        private const string DataTablePath = @"/GameMain/DataTables";
        private static readonly string DataTablePathConfig = Application.dataPath + @"/GameMain/Configs/DataTableConfig.xml";

        [MenuItem("Tools/Generate DataTables")]
        private static void GenerateDataTables()
        {
            PathBundle pathBundle = GetAllFileNames();
            for (int i = 0; i < pathBundle.fileNames.Length; i++)
            {
                string dataTableName = pathBundle.fileNames[i];
                string dataFilePath = pathBundle.filePaths[i];
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataFilePath, dataTableName);

                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }
        private class PathBundle
        {
            public string[] fileNames;
            public string[] filePaths;
            public PathBundle(string[] fileNames, string[] filePaths)
            {
                this.fileNames = fileNames;
                this.filePaths = filePaths;
            }
        }
        private static PathBundle GetAllFileNames()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + DataTablePath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.txt", SearchOption.AllDirectories);
            string[] fileNames = new string[fileInfos.Length];
            string[] filePaths = new string[fileInfos.Length];
            XElement root = new XElement("Root");
            XDocument xDocument = new XDocument(root);
            //获取开始的长度
            int startIndex = Application.dataPath.Length - "Assets".Length;
            for (int i = 0; i < fileNames.Length; i++)
            {
                filePaths[i] = fileInfos[i].FullName;
                fileNames[i] = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - ".txt".Length);
                root.Add(new XElement("DataTable", new XAttribute("Name", fileNames[i]), new XAttribute("FullName", filePaths[i].Substring(startIndex).Replace('\\', '/'))));
            }

            xDocument.Save(DataTablePathConfig);
            return new PathBundle(fileNames, filePaths);
        }
    }
}