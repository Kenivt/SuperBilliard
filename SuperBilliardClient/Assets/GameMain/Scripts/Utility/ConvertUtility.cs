using System;

namespace SuperBilliard
{
    public static class ConvertUtility
    {
        public static EnumBattle ToEnumBattle(this GameMessage.GameType gameType)
        {
            EnumBattle enumBattle = EnumBattle.None;
            switch (gameType)
            {
                case GameMessage.GameType.FancyMatch:
                    enumBattle = EnumBattle.FancyOnlineBattle;
                    break;
                case GameMessage.GameType.FancyFriend:
                    enumBattle = EnumBattle.FancyFriendBattle;
                    break;
                case GameMessage.GameType.SnookerMatch:
                    enumBattle = EnumBattle.SnokkerBattle;
                    break;
                case GameMessage.GameType.SnookerFriend:
                    enumBattle = EnumBattle.SnokkerFriendBattle;
                    break;
            }
            return enumBattle;
        }

        public static GameMessage.GameType ToGameType(this EnumBattle enumBattle)
        {
            GameMessage.GameType gameType = GameMessage.GameType.None;
            switch (enumBattle)
            {
                case EnumBattle.FancyOnlineBattle:
                    gameType = GameMessage.GameType.FancyMatch;
                    break;
                case EnumBattle.FancyFriendBattle:
                    gameType = GameMessage.GameType.FancyFriend;
                    break;
                case EnumBattle.SnokkerBattle:
                    gameType = GameMessage.GameType.SnookerMatch;
                    break;
                case EnumBattle.SnokkerFriendBattle:
                    gameType = GameMessage.GameType.SnookerFriend;
                    break;
            }
            return gameType;
        }

        public const string EnumLocalizationKeyTemplate = "ENUM_{0}_{1}";

        public static string EnumToLoaclizationKey<E>(E value) where E : Enum
        {
            Type type = typeof(E);
            string typeName = type.Name;
            string itemName = Enum.GetName(type, value);
            return string.Format(EnumLocalizationKeyTemplate, typeName.ToUpper(), itemName.ToUpper());
        }
    }
}