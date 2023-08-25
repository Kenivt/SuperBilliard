using GameMessage;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.Play
{
    public class MatchGameGroup : IGameGroup
    {
        public GameType GameType { get; private set; }

        public readonly Dictionary<int, IGameRoom> _roomDic;

        public readonly LinkedList<IGameRoom> _waittingList;

        public MatchGameGroup(GameType gameType)
        {
            GameType = gameType;

            _waittingList = new LinkedList<IGameRoom>();
            _roomDic = new Dictionary<int, IGameRoom>();
        }

        public GameRoomInfo[] GetAllRoomInfo()
        {
            GameRoomInfo[] gameRoomInfos = new GameRoomInfo[_roomDic.Count];
            int i = 0;
            foreach (var item in _roomDic)
            {
                IGameRoom room = item.Value;
                gameRoomInfos[i] = new GameRoomInfo
                {
                    playerCount = room.PlayerCount,
                    playerName1 = room.PlayerRoomStates[0] == null ? "default" : room.PlayerRoomStates[0].UserName,
                    playerName2 = room.PlayerRoomStates[1] == null ? "default" : room.PlayerRoomStates[1].UserName,
                    gameState = room.GameState
                };
                i++;
            }
            return gameRoomInfos;
        }

        public bool HasRoom(IGameRoom room)
        {
            return _roomDic.ContainsValue(room);
        }

        public bool HasRoom(int roomId)
        {
            return _roomDic.ContainsKey(roomId);
        }

        public void MatchGame(Player player)
        {
            if (_waittingList.Count == 0)
            {
                CreateRoom(player);
                return;
            }

            IGameRoom? gameRoom = null;
            bool flag = true;

            gameRoom = _waittingList.First.Value;
            flag = gameRoom.Enter(player);
            if (flag == true && gameRoom.GameState == GameState.Playering)
                _waittingList.RemoveFirst();
            //这是加入错误的情况
            if (flag == false)
            {
                Log.Error("Can't enter the Room,id is {0}.", gameRoom.RoomId);
                CreateRoom(player);
            }
        }

        public void RemoveAllRoom()
        {
            _waittingList.Clear();
            _roomDic.Clear();
        }

        public bool RemoveRoom(IGameRoom room)
        {
            bool flag1 = _waittingList.Remove(room);
            bool flag2 = _roomDic.Remove(room.RoomId);
            return flag2;
        }

        public IGameRoom CreateRoom(Player player)
        {
            GameRoom? gameRoom = null;

            int roomId = GeneratorId();

            gameRoom = GameRoom.Create(GameType, roomId);
            gameRoom.Enter(player);

            _roomDic.Add(roomId, gameRoom);
            _waittingList.AddFirst(gameRoom);
            return gameRoom;
        }

        public bool EnterRoom(Player player, int roomId)
        {
            return false;
        }

        //房间获取房间的Id
        private static int _roomId = 0;

        public static int GeneratorId()
        {
            return _roomId++;
        }

        public bool GetGameRoom(int roomId, out IGameRoom gameRoom)
        {
            return _roomDic.TryGetValue(roomId, out gameRoom);
        }
    }
}
