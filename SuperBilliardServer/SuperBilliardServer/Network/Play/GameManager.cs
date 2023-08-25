using GameMessage;
using ServerCore.Sington;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.Play
{
    public class GameManager : SingtonBase<GameManager>, IGameManager
    {
        private readonly Dictionary<GameType, IGameGroup> _matchGameDic = new Dictionary<GameType, IGameGroup>();

        public void RigisterGameGroup(IGameGroup gameGroup)
        {
            if (_matchGameDic.ContainsKey(gameGroup.GameType))
            {
                throw new Exception($"The game type {gameGroup.GameType} is already exist.");
            }
            _matchGameDic.Add(gameGroup.GameType, gameGroup);
        }

        /// <summary>
        /// 一般是好友联机的时候调用的添加房间的方法
        /// </summary>
        /// <param name="gameRoom"></param>

        public bool EnterRoom(Player player, GameType gameType, int roomid)
        {
            if (gameType == GameType.None)
            {
                return false;
            }
            if (_matchGameDic.ContainsKey(gameType))
            {
                return _matchGameDic[gameType].EnterRoom(player, roomid);
            }
            return false;
        }

        public GameRoomInfo[]? GetGameRoomInfo(GameType gameType)
        {
            if (_matchGameDic.ContainsKey(gameType))
            {
                return _matchGameDic[gameType].GetAllRoomInfo();
            }
            return null;
        }

        public (GameType, GameRoomInfo[]?)[] GetAllGameRoomInfo()
        {
            (GameType, GameRoomInfo[]?)[] values = new (GameType, GameRoomInfo[]?)[_matchGameDic.Count];
            int i = 0;
            foreach (var item in _matchGameDic)
            {
                int index = i++;
                values[index].Item1 = item.Value.GameType;
                values[index].Item2 = item.Value.GetAllRoomInfo();
            }
            return values;
        }

        public void MatchGame(Player player, GameType gameType)
        {
            if (_matchGameDic.ContainsKey(gameType))
            {
                _matchGameDic[gameType].MatchGame(player);
            }
        }

        public bool RemoveRoom(IGameRoom room)
        {
            if (_matchGameDic.ContainsKey(room.GameType))
            {
                return _matchGameDic[room.GameType].RemoveRoom(room);
            }
            return false;
        }

        public bool GetGameRoom(GameType gameType, int id, out IGameRoom gameRoom)
        {
            gameRoom = null;
            if (_matchGameDic.ContainsKey(gameType))
            {
                IGameGroup gameGroup = _matchGameDic[gameType];
                if (gameGroup.GetGameRoom(id, out gameRoom))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public IGameRoom CreateGameRoom(Player player, GameType gameType)
        {
            if (_matchGameDic.ContainsKey(gameType) == false)
            {
                throw new Exception($"The game type {gameType} is not exist.");
            }
            return _matchGameDic[gameType].CreateRoom(player);
        }
    }
}
