using GameFramework.Fsm;

namespace SuperBilliard
{
    public interface IBattleController
    {
        IFsm<IBattleController> Fsm { get; }

        void Init(IBattleData userdata);

        void ShutDown();

        void Reset();

        void StartPlaceWhiteBall();

        void StartOpponentTurn();
    }
}