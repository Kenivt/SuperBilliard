using GameFramework.DataTable;

namespace SuperBilliard
{
    /// <summary>
    /// 之前的设计失误，导致角色形象那块写的很乱
    /// </summary>
    public class PlayerImageDataBundle : DataBundleBase
    {

        protected override void OnPreload()
        {
            base.OnPreload();
            LoadDataTable("PlayerImage");
        }
    }
}