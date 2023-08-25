using GameMessage;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class FancyOnlineBattle : DefaultBattleBase
    {
        protected override void SetData(BattleDataItem battleDataItem, ref IBattleData battleData, ref IBattleController battleController,
          ref IBattleRuleSystem ruleSystem, ref IBattleBuilder battleBuilder)
        {
            battleData = new FancyBattleData(battleDataItem);
            battleController = new FancyOnlineBattleController();
            ruleSystem = new FancyOnlineRuleSystem();
            battleBuilder = new FancyOnlineBattleBuilder();
        }
    }
}