using System;
using System.IO;
using UnityGameFramework.Runtime;

namespace GFLearn.Editor.DataTableTools
{
    public partial class DataTableProcessor
    {
        public class IntArrayProcessor : GenericDataProcessor<int[]>
        {
            public override bool IsSystem => false;

            public override string LanguageKeyword => "int[]";

            public override string[] GetTypeStrings()
            {
                return new string[] {
                    "int[]",
                    "System.Int32[]"
                };
            }

            public override int[] Parse(string value)
            {
                if (value == "empty" || value == string.Empty)
                {
                    Log.Error("IntArrayProcessor并没有读取到对应的数据...");
                    return null;
                }
                string[] strs = value.Split(',');
                int[] ints = new int[strs.Length];
                try
                {
                    for (int i = 0; i < ints.Length; i++)
                    {
                        ints[i] = int.Parse(strs[i]);
                    }
                }
                catch (InvalidCastException e)
                {
                    Log.Error("错误,该值'{0}',无法被转换成Int,错误信息为{1}", value, e.Message);
                }
                return ints;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor,
                BinaryWriter binaryWriter, string value)
            {
                //写入数据...
                int[] intArray = Parse(value);
                binaryWriter.Write(intArray.Length);
                foreach (var elementValue in intArray)
                {
                    binaryWriter.Write(elementValue);
                }
            }
        }
    }
}