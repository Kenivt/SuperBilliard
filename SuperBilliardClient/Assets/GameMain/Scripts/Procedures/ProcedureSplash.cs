using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class ProcedureSplash : ProcedureBase
    {
        public override bool UseNativeDialog => true;
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (GameEntry.Base.EditorResourceMode)
            {
                ChangeState<ProcedurePreload>(procedureOwner);
            }
            else if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Package)
            {
                ChangeState<ProcedureInitlizeResources>(procedureOwner);
            }
            else if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Updatable)
            {
                Log.Warning("待开发中....");
            }
        }
    }
}