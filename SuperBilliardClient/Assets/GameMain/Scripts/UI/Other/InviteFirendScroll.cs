using Knivt.Tools.UI;

namespace SuperBilliard
{
    public class InviteFirendScroll : UICyclicScrollList<InviteFriendBar, FriendMessage>
    {
        protected override void ResetCellData(InviteFriendBar cell, FriendMessage data, int dataIndex)
        {
            cell.Display(data);
            cell.gameObject.SetActive(true);
        }
    }
}