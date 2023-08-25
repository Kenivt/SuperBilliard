using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SnokkerBattleController : IBattleController
    {
        public IFsm<IBattleController> Fsm { get; private set; }

        public string Name => this.GetType().Name;

        public void Init(IBattleData userdata)
        {
            if (GameEntry.Fsm.HasFsm<IBattleController>(Name))
            {
                Fsm = GameEntry.Fsm.GetFsm<IBattleController>(Name);
            }
            else
            {
                List<FsmState<IBattleController>> fsmStates = new List<FsmState<IBattleController>>()
                {
                    new NOBPlaceWhiteState(),
                    new NOBPrepareState(),
                    new NOBRollingState(),
                    new NOBMiddleState(),
                    new NOBOpponentTurnState(),
                    new NOBStorageState(),
                    new NOBTimeOutState(),
                };
                Fsm = GameEntry.Fsm.CreateFsm(Name, this, fsmStates);
            }
            //设置对应的数据
            Fsm.SetData<VarSingle>(NormalOnlineBattleStateBase.KeyMaxDragDistance, Screen.width / 6);
            Fsm.SetData<VarSingle>(NormalOnlineBattleStateBase.KeyMaxAngle, 30f);
            Fsm.SetData<VarSingle>(NormalOnlineBattleStateBase.KeyMaxVelocity, 18f);

            NormalOnlineBattleStateBase.BattleType = userdata.BattleType;
            VarObject rangeTypeObj = new VarObject();
            rangeTypeObj.Value = RangeType.SnokkerHalfCircle;
            Fsm.SetData(NormalOnlineBattleStateBase.KeyRangeType, rangeTypeObj);
            GameEntry.Event.Subscribe(StartTurnEventArgs.EventId, StartTurnCallback);
        }

        private void StartTurnCallback(object sender, GameEventArgs e)
        {
            StartTurnEventArgs args = e as StartTurnEventArgs;
            if (args == null)
            {
                Log.Error("StartTurnEventArgs is null");
                return;
            }
            if (args.IsPlaceWhite)
            {
                VarObject rangeTypeObj = new VarObject();
                rangeTypeObj.Value = RangeType.SnokkerHalfCircle;
                Fsm.SetData(NormalOnlineBattleStateBase.KeyRangeType, rangeTypeObj);
            }
        }

        public void ShutDown()
        {
            GameEntry.Fsm.DestroyFsm<IBattleController>(Name);
            GameEntry.Event.Unsubscribe(StartTurnEventArgs.EventId, StartTurnCallback);
        }

        public void StartPlaceWhiteBall()
        {
            Fsm.Start<NOBPlaceWhiteState>();
        }

        public void StartOpponentTurn()
        {
            Fsm.Start<NOBOpponentTurnState>();
        }

        public void Reset()
        {

        }
    }
}