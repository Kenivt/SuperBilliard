using Knivt.Tools.UI;

namespace SuperBilliard
{
    public class FirendMessageScroll : UICyclicScrollList<FriendMessageBar, FriendMessage>
    {
        protected override void ResetCellData(FriendMessageBar cell, FriendMessage data, int dataIndex)
        {
            if (cell == null)
            {
                return;
            }
            cell.gameObject.SetActive(true);
            cell.Display(data);
        }
    }
}
