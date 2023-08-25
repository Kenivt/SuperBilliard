using GameFramework.Fsm;

namespace SuperBilliard
{
    public abstract class NormalOnlineBattleStateBase : FsmState<IBattleController>
    {
        public const string KeyForce = "Energy";
        public const string KeyStorageSpeed = "StorageSpeed";
        public const string KeyMaxVelocity = "MaxVelocity";
        public const string KeyFillAmount = "FillAmount";
        public const string KeyFireDiraction = "Diraction";
        public const string KeyMaxAngle = "MaxAngle";
        public const string KeyMaxDragDistance = "MaxDragDistance";
        public const string KeyRangeType = "RangeType";
        public const string TagBilliard = "Ball";
        public const string KeyTimeOut = "TimeOut";
        public const string KeyGameType = "GameType";

        protected BilliardManager _billiardManager;
        public static EnumBattle BattleType { get; set; }
        protected override void OnEnter(IFsm<IBattleController> fsm)
        {
            base.OnEnter(fsm);
            _billiardManager = BilliardManager.Instance;
        }
    }
}