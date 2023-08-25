using System;
using System.Text;
using UnityEngine;
using Knivt.Tools.UI;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{


    public class PlayerImageScrollView : UICyclicScrollList<PlayerImageSlot, Sprite>
    {
        private readonly StringBuilder _keyCache = new StringBuilder();

        protected override void ResetCellData(PlayerImageSlot cell, Sprite sprite, int dataIndex)
        {
            if (cell == null)
            {
                return;
            }
            if (sprite == null)
            {
                return;
            }
            try
            {
                if (cell.gameObject.activeSelf == false)
                {
                    cell.gameObject.SetActive(true);
                }
                string key = sprite.name;
                int startIndex = key.IndexOf('_') + 1;
                //删除Clone
                int lastIndex = key.LastIndexOf('(');
                cell.PlayImageId = Convert.ToInt32(key.Substring(startIndex, lastIndex - startIndex));
                cell.Image.sprite = sprite;
            }
            catch (IndexOutOfRangeException)
            {
                Log.Error("在处理{0}该Sprite的时候索引越界,不符合'Name_Id'的命名规范.", sprite.name);
            }
            catch (InvalidCastException)
            {
                Log.Error("在处理{0}该Sprite的时候数据转换错误,不符合'Name_Id'的命名规范.", sprite.name);
            }
            catch (Exception ex)
            {
                Log.Error("在处理{0}该Sprite的时候发生错误,错误信息为{1}.", sprite.name, ex.Message);
            }
        }
    }
}
