using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace GFLearn.Editor.DataTableTools
{
    public partial class DataTableProcessor
    {
        private sealed class Vector3ListProcessor : GenericDataProcessor<List<Vector3>>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "List<Vector3>";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "List<Vector3>",
                    "unityengine.List<Vector3>"
                };
            }
            public override List<Vector3> Parse(string value)
            {
                List<Vector3> temp = new List<Vector3>();
                if (value == "empty" || value == string.Empty)
                {
                    return temp;
                }
                string[] splitValues = value.Split(';');
                for (int i = 0; i < splitValues.Length; i++)
                {
                    string[] splitedValue = splitValues[i].Split(',');
                    temp.Add(new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]
                        ), float.Parse(splitedValue[2])));
                }
                return temp;
            }
            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                List<Vector3> temp = Parse(value);
                binaryWriter.Write((float)temp.Count);
                for (int i = 0; i < temp.Count; i++)
                {
                    binaryWriter.Write(temp[i].x);
                    binaryWriter.Write(temp[i].y);
                    binaryWriter.Write(temp[i].z);
                }
            }
        }
    }

}
