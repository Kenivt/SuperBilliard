using System;
using UnityEngine;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public static class DataTableExtension
    {
        private const string DataRowCalssPrefixName = "SuperBilliard.DR";
        internal static readonly char[] DataSplitSeparators = new char[] { '\t' };
        //因为从Excel中导出数据时，有些数据会用“”来做分割
        internal static readonly char[] DataTrimSeparators = new char[] { '\"' };
        public static void LoadDataTable(this DataTableComponent dataTableComponent
            , string dataTableName, string dataTableAssetName, object userData)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                //invalid 无效的
                Log.Warning("Data table name is invalid.");
                return;
            }
            //大概是检查命名规范
            string[] splitedNames = dataTableName.Split('_');
            if (splitedNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }
            //拼接类名
            string dataRowClassName = DataRowCalssPrefixName + splitedNames[0];
            Type dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning("Can not get date row with class name'{0}'.", dataRowClassName);
            }

            string name = splitedNames.Length > 1 ? splitedNames[1] : null;
            DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
            //读取数据...
            dataTable.ReadData(dataTableAssetName, SuperBilliard.Constant.AssetPriority.DataTableAsset, userData);
        }
        public static Color32 ParseColor32(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color32(byte.Parse(splitedValue[0]), byte.Parse(splitedValue[1]), byte.Parse(splitedValue[2]), byte.Parse(splitedValue[3]));
        }

        public static Color ParseColor(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Quaternion ParseQuaternion(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Quaternion(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Rect ParseRect(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Rect(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Vector2 ParseVector2(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector2(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]));
        }

        public static Vector3 ParseVector3(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
        }

        public static Vector4 ParseVector4(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector4(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static int[] ParseArrayInt(string value)
        {
            if (value == "empty" || value == string.Empty)
            {
                Log.Error("没有数据...");
                return null;
            }
            string[] splitedValues = value.Split(',');
            int[] temp = new int[splitedValues.Length];
            try
            {
                for (int i = 0; i < splitedValues.Length; i++)
                {
                    temp[i] = int.Parse(splitedValues[i]);
                }
            }
            catch (InvalidCastException ex)
            {
                Log.Error("错误,不能把对应的值‘{0}’转化为整数,错误信息为{1}", value, ex.Message);
                throw;
            }
            return temp;
        }
        public static List<Vector3> ParseListVector3(string value)
        {
            if (value == "empty" || value == string.Empty)
            {
                Log.Warning("没有数据...");
                return new List<Vector3>();
            }
            string[] splitedValues = value.Split(';');
            List<Vector3> temp = new List<Vector3>(splitedValues.Length);
            for (int i = 0; i < splitedValues.Length; i++)
            {
                temp.Add(ParseVector3(splitedValues[i]));
            }
            return temp;
        }
    }
}


