namespace SuperBilliard.Constant
{
    public static class PacketTypeId
    {
        public const int HeartBeat = 1;
        public const int Confirm = 2;

        //打开KCP
        public const int CSOpenKcp = 101;
        public const int SCOpenKcp = 102;

        //UserData
        public const int UserData = 201;
        public const int Rigister = 202;
        public const int Login = 203;
        public const int SCPlayerMessage = 204;
        public const int CSGetPlayerMessage = 205;
        public const int CSSavePlayerMessage = 206;
        public const int CSGetFriendMessage = 207;
        public const int SCFriendListMessage = 208;
        public const int CSRequestFriend = 209;
        public const int SCRequestFriend = 210;
        public const int SCReturnMessage = 211;
        public const int CSGetFriendRequestList = 212;
        public const int SCFriendRequestList = 213;

        public const int CSProcessingFriendRequest = 214;
        public const int SCProcessingFriendRequest = 215;

        public const int SCUpdateFriendState = 220;
        //Battle
        public const int CSBilliardSync = 301;
        public const int SCBilliardSync = 302;
        public const int CSCueSync = 303;
        public const int SCCueSync = 304;
        public const int SCCueStorageSync = 305;
        public const int CSCueStorageSync = 306;
        public const int CSExitGameRoom = 307;
        public const int CSConfirmBilliardType = 308;
        public const int SCConfirmBilliardType = 309;

        public const int CSSetBilliardState = 310;
        public const int SCSetBilliardState = 311;

        public const int CSSyncSound = 312;
        public const int SCSyncSound = 313;

        public const int CSBattleEmptyEvent = 314;
        public const int SCBattleEmptyEvent = 315;

        public const int CSEndTurn = 316;
        public const int SCStartTurn = 317;
        public const int CSTurnAnalysis = 318;
        public const int SCTurnAnalysis = 319;


        //MatchGame
        public const int CSJoinGame = 401;
        public const int SCGameStart = 402;
        public const int GameResult = 403;
        public const int CSStopMatch = 404;

        public const int CSLoadGameComplete = 405;
        public const int SCStartLoadGame = 406;

        //FriendRoom
        public const int CSInviteFriendBattle = 501;
        public const int SCInviteFriendBattle = 502;
        public const int CSAcceptGameInvitation = 503;
        public const int SCAcceptGameInvitation = 504;
        public const int SCInviteeEnterRoom = 505;

        public const int CSLeaveFriendRoom = 507;
        public const int SCLeaveFriendRoom = 508;
        public const int CSReadyGame = 509;
        public const int SCReadyGame = 510;

        public const int CSCreateGame = 511;
        public const int SCCreateGame = 512;
    }
}
