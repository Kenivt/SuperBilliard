using System;
using GameFramework.Event;

namespace SuperBilliard
{
    public class SnokkerBattle : DefaultBattleBase
    {
        protected override void SetData(BattleDataItem battleDataItem,
            ref IBattleData battleData, ref IBattleController battleController,
            ref IBattleRuleSystem ruleSystem, ref IBattleBuilder battleBuilder)
        {
            battleData = new SnokkerData(battleDataItem.BattleType, battleDataItem.RandomSeed);
            battleController = new SnokkerBattleController();
            ruleSystem = new SnokkerRuleSystem();
            battleBuilder = new SnokkerBuilder();
        }
    }
}