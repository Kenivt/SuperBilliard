using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.Play
{
    public class FirendGameGroup : IGameGroup
    {
        public GameType GameType { get; private set; }

        public FirendGameGroup(GameType gameType)
        {
            GameType = gameType;
        }

        private readonly Dictionary<int, IGameRoom> _roomDic = new Dictionary<int, IGameRoom>();

        public IGameRoom CreateRoom(Player player)
        {
            int roomId = GeneratorGameId();
            FriendGameRoom firendGameRoom = FriendGameRoom.Create(GameType, roomId);
            _roomDic.Add(roomId, firendGameRoom);
            player.GameRoom = firendGameRoom;
            firendGameRoom.Enter(player);
            return firendGameRoom;
        }

        public bool EnterRoom(Player player, int roomId)
        {
            if (_roomDic.ContainsKey(roomId))
            {
                return _roomDic[roomId].Enter(player);
            }
            else
            {
                Log.Error("不存在对应的房间");
                return false;
            }
        }

        public GameRoomInfo[] GetAllRoomInfo()
        {
            GameRoomInfo[] roomInfos = new GameRoomInfo[_roomDic.Count];
            int index = 0;
            foreach (var room in _roomDic.Values)
            {
                roomInfos[index++] = new GameRoomInfo
                {
                    gameState = room.GameState,
                    playerCount = room.PlayerCount,
                    playerName1 = room.PlayerRoomStates[0] == null ? "default" : room.PlayerRoomStates[0].UserName,
                    playerName2 = room.PlayerRoomStates[1] == null ? "default" : room.PlayerRoomStates[1].UserName,
                };
            }
            return roomInfos.ToArray();
        }

        public bool HasRoom(IGameRoom room)
        {
            return _roomDic.ContainsKey(room.RoomId);
        }

        public bool HasRoom(int roomId)
        {
            return _roomDic.ContainsKey(roomId);
        }

        public void MatchGame(Player player)
        {
            Log.Error("不支持匹配模式");
        }

        public void RemoveAllRoom()
        {
            _roomDic.Clear();
        }

        public bool RemoveRoom(IGameRoom room)
        {
            return _roomDic.Remove(room.RoomId);
        }

        static int _gameId;

        private static int GeneratorGameId()
        {
            return _gameId++;
        }

        public bool GetGameRoom(int roomId, out IGameRoom gameRoom)
        {
            if (_roomDic.ContainsKey(roomId))
            {
                gameRoom = _roomDic[roomId];
                return true;
            }
            else
            {
                gameRoom = null;
                return false;
            }
        }
    }
}

