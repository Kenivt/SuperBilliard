using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.Play
{
    public class FriendGameRoom : IGameRoom
    {
        public int RoomId { get; set; }

        public int PlayerCount { get; private set; }

        public GameState GameState { get; set; }

        public int RoomSeed { get; private set; }
        public PlayerRoomState[] PlayerRoomStates
        {
            get
            {
                return _roomStates;
            }
        }

        public GameType GameType { get; private set; }

        private PlayerRoomState[] _roomStates = new PlayerRoomState[2];

        public bool Enter(Player player)
        {
            //重复添加...
            for (int i = 0; i < _roomStates.Length; i++)
            {
                if (_roomStates[i] != null && _roomStates[i].player == player)
                {
                    Log.Warning("repeated add client.");
                    return true;
                }
            }
            for (int i = 0; i < _roomStates.Length; i++)
            {
                if (PlayerRoomStates[i] == null)
                {
                    PlayerRoomStates[i] = PlayerRoomState.Create(player);
                    player.OnEnterGameRoom(this);
                    PlayerCount++;
                    return true;
                }
            }
            return false;
        }

        public bool Leave(Player player)
        {
            for (int i = 0; i < _roomStates.Length; i++)
            {
                if (PlayerRoomStates[i] != null && PlayerRoomStates[i].player == player)
                {
                    player.OnLeaveGameRoom();
                    ReferencePool.Release(PlayerRoomStates[i]);
                    PlayerRoomStates[i] = null;
                    PlayerCount--;

                    if (GameState == GameState.Playering)
                    {
                        //发送游戏结束
                        SCGameResultPack pac = ReferencePool.Acquire<SCGameResultPack>();
                        pac.Result = GameResult.Victory;
                        SendMessageToAnother(player, pac);
                        ReferencePool.Release(pac);
                    }

                    if (PlayerCount == 0)
                    {
                        GameManager.Instance.RemoveRoom(this);
                    }
                    //回收一下
                    GameState = GameState.Waitting;
                    return true;
                }
            }
            return false;
        }

        public void ReadyGame(Player player, bool isReady)
        {
            for (int i = 0; i < _roomStates.Length; i++)
            {
                if (PlayerRoomStates[i] != null && PlayerRoomStates[i].player == player)
                {
                    PlayerRoomStates[i].isReady = isReady;
                }
            }
            //判断是否全部准备好了
            bool isAllReady = false;
            for (int i = 0; i < _roomStates.Length; i++)
            {
                if (PlayerRoomStates[i] == null || PlayerRoomStates[i].isReady == false)
                {
                    isAllReady = false;
                    break;
                }
                else
                {
                    isAllReady = true;
                }
            }
            //全部准备好了,开始游戏
            if (isAllReady)
            {
                StartLoadGame();
            }
        }

        public void StartLoadGame()
        {
            GameState = GameState.Playering;

            ////判定先手
            //Random random = new Random();
            //int index = random.Next(0, 2);

            SCStartLoadGame sCGameStartPack = SCStartLoadGame.Create();
            //获得随机种子
            RoomSeed = new Random().Next(114514, 229028);
            //设置随机种子
            sCGameStartPack.RandomSeed = RoomSeed;
            sCGameStartPack.GameType = GameType;
            sCGameStartPack.IsFirstMove = new Random().Next(0, 2) == 1 ? true : false;

            sCGameStartPack.OpponentName = PlayerRoomStates[1].UserName;
            PlayerRoomStates[0].SendPacket(sCGameStartPack);

            sCGameStartPack.OpponentName = PlayerRoomStates[0].UserName;
            sCGameStartPack.IsFirstMove = !sCGameStartPack.IsFirstMove;
            PlayerRoomStates[1].SendPacket(sCGameStartPack);

            ReferencePool.Release(sCGameStartPack);
        }

        public void EndGame(GameResult gameResult, Player player)
        {
            GameState = GameState.End;
            SCGameResultPack sCGameResultPack = ReferencePool.Acquire<SCGameResultPack>();
            //发送结束游戏的消息
            if (PlayerRoomStates[0].player == player)
            {
                if (gameResult == GameResult.Victory)
                {
                    sCGameResultPack.Result = GameResult.Victory;
                    PlayerRoomStates[0].SendPacket(sCGameResultPack);
                    sCGameResultPack.Result = GameResult.Defeat;
                    PlayerRoomStates[1].SendPacket(sCGameResultPack);
                }
                else
                {
                    sCGameResultPack.Result = GameResult.Defeat;
                    PlayerRoomStates[0].SendPacket(sCGameResultPack);
                    sCGameResultPack.Result = GameResult.Victory;
                    PlayerRoomStates[1].SendPacket(sCGameResultPack);
                }
            }
            else
            {
                if (gameResult == GameResult.Victory)
                {
                    sCGameResultPack.Result = GameResult.Defeat;
                    PlayerRoomStates[0].SendPacket(sCGameResultPack);
                    sCGameResultPack.Result = GameResult.Victory;
                    PlayerRoomStates[1].SendPacket(sCGameResultPack);
                }
                else
                {
                    sCGameResultPack.Result = GameResult.Victory;
                    PlayerRoomStates[0].SendPacket(sCGameResultPack);
                    sCGameResultPack.Result = GameResult.Defeat;
                    PlayerRoomStates[1].SendPacket(sCGameResultPack);
                }
            }

            ReferencePool.Release(sCGameResultPack);
        }

        public bool Broadcast(Packet packet)
        {
            if (packet == null)
            {
                Log.Error("packet is invalid.");
                return false;
            }

            bool flag = true;

            for (int i = 0; i < _roomStates.Length; i++)
            {
                if (PlayerRoomStates[i] != null)
                {
                    PlayerRoomStates[i].SendPacket(packet);
                }
            }
            return flag;
        }

        public bool SendMessageToAnother(Player sendPlayer, Packet packet)
        {
            foreach (var item in PlayerRoomStates)
            {
                if (item == null || item.player == sendPlayer)
                {
                    continue;
                }
                item.SendPacket(packet);
            }
            return true;
        }

        public bool SendKcpMessageToAnother(Player sendClient, Packet packet)
        {
            foreach (var item in PlayerRoomStates)
            {
                if (item == null || item.player == sendClient)
                {
                    continue;
                }
                item.SendKcpPacket(packet);
            }
            return true;
        }

        public static FriendGameRoom Create(GameType gameType, int roomId)
        {
            FriendGameRoom gameRoom = ReferencePool.Acquire<FriendGameRoom>();
            gameRoom.GameType = gameType;
            gameRoom.GameState = GameState.Waitting;
            gameRoom.RoomId = roomId;
            return gameRoom;
        }

        public void Clear()
        {
            RoomId = 0;
            PlayerCount = 0;
            for (int i = 0; i < PlayerRoomStates.Length; i++)
            {
                if (PlayerRoomStates[i] != null)
                {
                    ReferencePool.Release(PlayerRoomStates[i]);
                    PlayerRoomStates[i] = null;
                }
            }
            GameState = GameState.End;
        }

        public void LoadGameComplete(Player player)
        {
            bool flag = true;
            foreach (var item in PlayerRoomStates)
            {
                if (item == null)
                {
                    Log.Error("加载游戏的过程中有玩家离开...");
                    return;
                }
                if (item.player == player)
                {
                    item.isLoadGameComplete = true;
                }
                if (item.isLoadGameComplete == false)
                    flag = false;
            }
            //所有玩家加载完成了
            if (flag)
            {
                //广播游戏开始的消息
                var packet = SCGameStart.Create();
                Broadcast(packet);
                ReferencePool.Release(packet);
            }
        }
    }
}