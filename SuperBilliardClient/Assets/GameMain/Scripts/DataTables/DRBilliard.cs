﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-08-24 15:01:38.954
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
    public class DRBilliard : DataRowBase
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
        /// 获取。
        /// </summary>
        public int BilliardId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取台球对应的名称。
        /// </summary>
        public string BilliardName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取对应SpriteAtlas的Id。
        /// </summary>
        public int SpriteAtlasId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public Vector3 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public Vector3 EulerAngle
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public int MaterialAssetId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取这一行似乎没什么用处。
        /// </summary>
        public int Score
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
            BilliardId = int.Parse(columnStrings[index++]);
            BilliardName = columnStrings[index++];
            SpriteAtlasId = int.Parse(columnStrings[index++]);
            Position = DataTableExtension.ParseVector3(columnStrings[index++]);
            EulerAngle = DataTableExtension.ParseVector3(columnStrings[index++]);
            MaterialAssetId = int.Parse(columnStrings[index++]);
            Score = int.Parse(columnStrings[index++]);

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
                    BilliardId = binaryReader.Read7BitEncodedInt32();
                    BilliardName = binaryReader.ReadString();
                    SpriteAtlasId = binaryReader.Read7BitEncodedInt32();
                    Position = binaryReader.ReadVector3();
                    EulerAngle = binaryReader.ReadVector3();
                    MaterialAssetId = binaryReader.Read7BitEncodedInt32();
                    Score = binaryReader.Read7BitEncodedInt32();
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
