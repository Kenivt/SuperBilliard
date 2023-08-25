using Knivt.Tools.UI;

namespace SuperBilliard
{
    public class AddFirendMessageScroll : UICyclicScrollList<AddFirendMessageBar, AddFriendMessageData>
    {
        protected override void ResetCellData(AddFirendMessageBar cell, AddFriendMessageData data, int dataIndex)
        {
            if (cell == null)
                return;
            cell.gameObject.SetActive(true);
            cell.Display(data);
            return;
        }
    }
}