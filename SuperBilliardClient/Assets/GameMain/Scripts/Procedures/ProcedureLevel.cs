using System;
using GameMessage;
using GameFramework.Fsm;
using GameFramework.Event;
using SuperBilliard.Constant;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class ProcedureLevel : ProcedureBase
    {
        private IBattle _battle;

        private bool _backMainMenu = false;

        public override bool UseNativeDialog => true;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(BackMainMenuEventArgs.EventId, OnBackMainMenu);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId, OnGameOver);

            _backMainMenu = false;

            //显示固定的UI
            GameEntry.UI.OpenUIForm(EnumUIForm.StorageBar);
            GameEntry.UI.OpenUIForm(EnumUIForm.MessageUIForm);
            GameEntry.UI.OpenUIForm(EnumUIForm.BattleTipUIForm);

            int battleId = procedureOwner.GetData<VarInt32>(KeyBattle);
            BattleDataBundle battleDataBundle = GameEntry.DataBundle.GetData<BattleDataBundle>();

            Type battleType = battleDataBundle.GetBattleType(battleId);
            _battle = (IBattle)Activator.CreateInstance(battleType);
            _battle.Init(battleDataBundle.GetBattleDataItem(battleId));
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(BackMainMenuEventArgs.EventId, OnBackMainMenu);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, OnGameOver);
            //重置一下.
            GameEntry.DataBundle.GetData<FriendRoomDataBundle>().Reset();
            _battle.ShutDown();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            _battle.Update(elapseSeconds, realElapseSeconds);

            if (_battle.State == BattleState.GameOver)
            {
                if (_backMainMenu == true)
                {
                    procedureOwner.SetData<VarInt32>(KeyNextSceneId, (int)SceneId.MainMenu);
                    ChangeState<ProcedureChangeScene>(procedureOwner);
                }
            }
        }

        private void OnGameOver(object sender, GameEventArgs e)
        {
            GameOverEventArgs ne = (GameOverEventArgs)e;
            _battle.State = BattleState.GameOver;

            if (ne.GameResult == GameResult.Defeat)
            {
                GameEntry.Sound.PlaySound(EnumSound.Defeated);
            }
            else if (ne.GameResult == GameResult.Victory)
            {
                GameEntry.Sound.PlaySound(EnumSound.Victory);
            }

            GameEntry.UI.OpenUIForm(EnumUIForm.GameOverUIForm, ne.GameResult);
        }

        private void OnBackMainMenu(object sender, GameEventArgs e)
        {
            BackMainMenuEventArgs args = (BackMainMenuEventArgs)e;
            _backMainMenu = true;
        }
    }
}