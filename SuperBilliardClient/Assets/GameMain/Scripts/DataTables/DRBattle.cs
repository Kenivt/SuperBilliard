//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-08-24 15:01:38.949
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    /// <summary>
    /// 资源路径配置表。
    /// </summary>
    public class DRBattle : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取资源编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取战斗名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取场景Id。
        /// </summary>
        public int SceneId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对战类型。
        /// </summary>
        public string BattleTypeName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应的台球ID。
        /// </summary>
        public int[] BilliardIdArray
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Name = columnStrings[index++];
            SceneId = int.Parse(columnStrings[index++]);
            BattleTypeName = columnStrings[index++];
            BilliardIdArray = DataTableExtension.ParseArrayInt(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Name = binaryReader.ReadString();
                    SceneId = binaryReader.Read7BitEncodedInt32();
                    BattleTypeName = binaryReader.ReadString();
                    BilliardIdArray = binaryReader.ReadArrayInt();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
