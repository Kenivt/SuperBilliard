using GameFramework;
using GameMessage;

namespace SuperBilliard
{
    public enum BattleState
    {
        None,
        Playing,
        GameOver,
    }

    public interface IBattle
    {
        BattleDataItem Battle { get; }
        BattleState State { get; set; }
        void Init(BattleDataItem battleItem);
        void Update(float elaspSeconds, float realElaspSeconds);

        void Restart();
        void ShutDown();
    }
}