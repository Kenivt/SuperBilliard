using Knivt.Tools.UI;
using UnityEngine;
using UnityGameFramework;
namespace SuperBilliard
{
    public struct TestScrollData
    {
        public string Name;
        public int Age;
    }
    public class TestScrollView : UICyclicScrollList<TestUIItem, TestScrollData>
    {
        private void Awake()
        {

        }

        protected override void ResetCellData(TestUIItem cell, TestScrollData data, int dataIndex)
        {

        }
    }
}