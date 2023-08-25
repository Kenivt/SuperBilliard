using Knivt.Tools.UI;

namespace SuperBilliard
{
    public class FriendRequestScroll : UICyclicScrollList<FriendRequestBar, FriendRequestData>
    {
        protected override void ResetCellData(FriendRequestBar cell, FriendRequestData data, int dataIndex)
        {
            if (cell == null)
            {
                return;
            }
            cell.Display(data);
            cell.gameObject.SetActive(true);
        }
    }
}
