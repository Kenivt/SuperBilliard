namespace SuperBilliard
{
    public interface IBattleRuleSystem
    {
        void Init(IBattleData userData);

        void ShutDown();

        void Update(float elapseSeconds, float realElapseSeconds);

        void Reset();
    }
}