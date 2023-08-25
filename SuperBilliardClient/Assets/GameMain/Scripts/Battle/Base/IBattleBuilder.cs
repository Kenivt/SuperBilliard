using System;

namespace SuperBilliard
{
    public interface IBattleBuilder
    {
        bool IsComplete { get; }

        event Action OnComplete;

        void Build(BattleDataItem userData);

        void Reset();

        void Update(float elasp, float real);
    }
}