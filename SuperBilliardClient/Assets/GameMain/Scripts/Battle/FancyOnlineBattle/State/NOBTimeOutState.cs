using UnityEngine;
using GameFramework.Fsm;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class NOBTimeOutState : NormalOnlineBattleStateBase
    {
        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            int x = UnityEngine.Random.Range(-1, 1);
            int z = UnityEngine.Random.Range(-1, 1);
            Vector3 dir = new Vector3(x, 0, z).normalized;
            fsm.SetData<VarVector3>(KeyFireDiraction, dir);
            fsm.SetData<VarSingle>(KeyFillAmount, 0.05f);
        }
        protected override void OnUpdate(IFsm<IBattleController> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            ChangeState<NOBRollingState>(fsm);
        }
    }
}