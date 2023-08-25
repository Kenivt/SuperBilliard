using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class ProcedureInitlizeResources : ProcedureBase
    {
        public override bool UseNativeDialog => true;
        public bool ResouceInitlized { get; private set; }


        protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("初始化资源..");
            GameEntry.Resource.InitResources(InitlizeResCallback);
        }
        protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (ResouceInitlized == false)
            {
                return;
            }
            ChangeState<ProcedurePreload>(procedureOwner);
        }
        private void InitlizeResCallback()
        {
            ResouceInitlized = true;
        }

        protected override void OnLeave(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}