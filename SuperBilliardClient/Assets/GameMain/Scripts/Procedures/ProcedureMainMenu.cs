using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class ProcedureMainMenu : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        private bool _gameStart;
        private int? _invitationId;
        private IFsm<GameFramework.Procedure.IProcedureManager> _procedureOwner;

        protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _invitationId = null;

            GameEntry.UI.OpenUIForm(EnumUIForm.MainMenuUIForm);
            GameEntry.UI.OpenUIForm(EnumUIForm.GameModeUIForm);

            GameEntry.Event.Subscribe(StartLoadGameEventArgs.EventId, OnStartLoadGame);
            GameEntry.Event.Subscribe(RecieveBattleInvitationEventArgs.EventId, OnRecieveBattleInvitation);

            _procedureOwner = procedureOwner;
            _gameStart = false;
        }

        protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (_gameStart == true)
            {
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(StartLoadGameEventArgs.EventId, OnStartLoadGame);
            GameEntry.Event.Unsubscribe(RecieveBattleInvitationEventArgs.EventId, OnRecieveBattleInvitation);
        }

        private void OnRecieveBattleInvitation(object sender, GameEventArgs e)
        {
            RecieveBattleInvitationEventArgs ne = (RecieveBattleInvitationEventArgs)e;
            //未打开的时候
            if (_invitationId == null || GameEntry.UI.HasUIForm(_invitationId.Value) == false)
            {
                InvitationMessage message = InvitationMessage.Create(ne.InviterUsername, ne.RoomId, ne.GameType);
                _invitationId = GameEntry.UI.OpenUIForm(EnumUIForm.FriendInvitation, message);
            }
            else
            {
                UIForm uIForm = GameEntry.UI.GetUIForm(_invitationId.Value);
                FriendInvitationUIForm logic = uIForm.Logic as FriendInvitationUIForm;
                //推入队列
                logic.InvitationMessageQueue.Enqueue(InvitationMessage.Create(ne.InviterUsername, ne.RoomId, ne.GameType));
            }
        }

        private void OnStartLoadGame(object sender, GameEventArgs e)
        {
            StartLoadGameEventArgs args = e as StartLoadGameEventArgs;

            if (args.GameType == EnumBattle.None)
            {
                Log.Error("GameType is None");
                return;
            }
            var databundle = GameEntry.DataBundle.GetData<BattleDataBundle>();

            var bundle = databundle.GetBattleDataItem((int)args.GameType);
            int sceneId = databundle.GetSceneId(args.GameType);
            _gameStart = true;

            //设置对应的数据
            bundle.IsFristMove = args.IsFirstMove;
            bundle.RandomSeed = args.RandomSeed;

            _procedureOwner.SetData<VarInt32>(KeyNextSceneId, sceneId);
            _procedureOwner.SetData<VarInt32>(KeyBattle, (int)args.GameType);

            int uiserilizeId = GameEntry.UI.OpenUIForm(EnumUIForm.GameStartUIForm, args.OpponentUserName).Value;
            _ignoreUIQueue.Enqueue(uiserilizeId);
        }
    }
}