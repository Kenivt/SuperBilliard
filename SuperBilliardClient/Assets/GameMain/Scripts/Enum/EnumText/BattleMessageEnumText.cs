using UnityEngine;
using UnityGameFramework;

namespace SuperBilliard
{
    [LoaclizationEnumText]
    public enum BattleMessageEnumText
    {
        [LoaclizationEnumAdditional("FOUL!! TURN END")]
        Foul,
        [LoaclizationEnumAdditional("SCORE! TURN RESTART")]
        Score,
        [LoaclizationEnumAdditional("GAME TIED!! THE WINNING BALL IS PLACED!!")]
        GameTied,
        Victory,
        Defeated,
        [LoaclizationEnumAdditional("TIME OUT! TURN END")]
        TimeOut,
        [LoaclizationEnumAdditional("TURN START.PLEASE PLACE WHITE.")]
        PlaceWhiteBall,
        [LoaclizationEnumAdditional("FOUL.WHITE ENTER HOLE!")]
        WhiteEnterHole,
        [LoaclizationEnumAdditional("FOUL.EIGHT ENTER HOLE,GAME OVER!!")]
        EightEnterHole,
        [LoaclizationEnumAdditional("FRIST COLLIDE FOUL.TURN END.")]
        FristCollideFoul,
        [LoaclizationEnumAdditional("WHITE COLLIDE EDGE.TURN END.")]
        WhiteCollideEdge,
        [LoaclizationEnumAdditional("THE WHITE BALL DID'T TOUCH ANYTHINE.")]
        WhiteNoCollde,
        [LoaclizationEnumAdditional("{0} FOULS .IF MORE THAN {1} WILL END THE GAME.")]
        FoulCountFormat,
        [LoaclizationEnumAdditional("CONFIRM BILLIARD TYPE.")]
        ConfirmBilliardType,
        [LoaclizationEnumAdditional("START TURN.")]
        StartTurn,
        [LoaclizationEnumAdditional("END TURN.")]
        EndTurn,
    }
}