namespace SuperBilliard
{
    public interface IBattleData
    {
        System.Random PostionRandom { get; }
        EnumBattle BattleType { get; }
        bool IsOwnTurn { get; set; }
        void Reset();
    }
}