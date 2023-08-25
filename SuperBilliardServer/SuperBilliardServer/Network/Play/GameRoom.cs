using GameMessage;
using GameFramework;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.Play
{
    public class GameRoom : IGameRoom
    {
        public int RoomId { get; set; }

        public int PlayerCount { get; private set; }

        public GameState GameState { get; set; }

        public PlayerRoomState[] PlayerRoomStates => _player;

        public GameType GameType { get; private set; }

        public int RoomSeed { get; private set; }

        private readonly PlayerRoomState[] _player = new PlayerRoomState[2];

        public bool Enter(Player player)
        {
            player.State = PlayerStatus.Watting;
            for (int i = 0; i < _player.Length; i++)
            {
                if (_player[i] != null && _player[i].player == player)
                {
                    Log.Warning("repeated add client.");
                    return true;
                }
            }

            for (int i = 0; i < _player.Length; i++)
            {
                if (_player[i] == null)
                {
                    _player[i] = PlayerRoomState.Create(player);
                    player.OnEnterGameRoom(this);
                    PlayerCount++;
                    //房间满了之后开始游戏

                    if (PlayerCount == 2) StartLoadGame();
                    return true;
                }
            }
            return false;
        }

        public bool Leave(Player player)
        {
            player.State = PlayerStatus.Idle;

            for (int i = 0; i < _player.Length; i++)
            {
                if (PlayerRoomStates[i] != null && PlayerRoomStates[i].player == player)
                {
                    ReferencePool.Release(PlayerRoomStates[i]);
                    PlayerRoomStates[i] = null;
                    PlayerCount--;

                    player.OnLeaveGameRoom();
                    //如果当前游戏处于游玩中则通知对方游戏结束
                    if (GameState == GameState.Playering)
                    {
                        //发送游戏结束
                        SCGameResultPack pac = ReferencePool.Acquire<SCGameResultPack>();
                        pac.Result = GameResult.Victory;
                        SendMessageToAnother(player, pac);
                        ReferencePool.Release(pac);
                    }
                    GameState = GameState.Waitting;

                    if (PlayerCount == 0)
                    {
                        if (GameManager.Instance.RemoveRoom(this) == false)
                        {
                            Log.Warning("移除房间失败...");
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Broadcast(Packet packet)
        {
            if (packet == null)
            {
                Log.Error("packet is invalid.");
                return false;
            }

            bool flag = true;

            for (int i = 0; i < _player.Length; i++)
            {
                if (PlayerRoomStates[i] != null)
                {
                    PlayerRoomStates[i].SendPacket(packet);
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }

        public bool SendMessageToAnother(Player sendClient, Packet packet)
        {
            foreach (var item in PlayerRoomStates)
            {
                if (item == null || item.player == sendClient)
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

        public void StartLoadGame()
        {
            GameState = GameState.Playering;

            //随机先手
            //Random random = new Random();
            //int index = random.Next(0, 2);
            //index == 0 ? true : false;

            //开始加载游戏
            SCStartLoadGame packet = SCStartLoadGame.Create();
            //获得随机种子
            RoomSeed = new Random().Next(114514, 229028);
            //设置随机种子
            packet.RandomSeed = RoomSeed;
            packet.GameType = GameType;

            packet.IsFirstMove = new Random().Next(0, 2) == 1 ? true : false;

            packet.OpponentName = PlayerRoomStates[1].UserName;
            PlayerRoomStates[0].SendPacket(packet);

            packet.OpponentName = PlayerRoomStates[0].UserName;
            packet.IsFirstMove = !packet.IsFirstMove;
            PlayerRoomStates[1].SendPacket(packet);

            for (int i = 0; i < PlayerRoomStates.Length; i++)
            {
                PlayerRoomStates[i].player.State = PlayerStatus.Gaming;
            }
            ReferencePool.Release(packet);
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

        public static GameRoom Create(GameType gameType, int roomId)
        {
            GameRoom gameRoom = ReferencePool.Acquire<GameRoom>();
            gameRoom.GameType = gameType;
            gameRoom.GameState = GameState.Waitting;
            gameRoom.RoomId = roomId;
            return gameRoom;
        }

        public void ReadyGame(Player player, bool isReady)
        {
            throw new NotImplementedException();
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

        public void Clear()
        {
            RoomId = -1;
            _player[0] = null;
            _player[1] = null;
            PlayerCount = 0;
            GameState = GameState.End;
            //重置一下
            for (int i = 0; i < _player.Length; i++)
            {
                if (PlayerRoomStates[i] != null)
                {
                    ReferencePool.Release(PlayerRoomStates[i]);
                    PlayerRoomStates[i] = null;
                }
            }
        }
    }
}