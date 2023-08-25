using GameMessage;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.Play
{
    public interface IGameGroup
    {
        /// <summary>
        /// 游戏的类型
        /// </summary>
        public GameType GameType { get; }

        /// <summary>
        /// 匹配游戏
        /// </summary>
        /// <param name="player"></param>
        void MatchGame(Player player);

        /// <summary>
        /// 移除房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        bool RemoveRoom(IGameRoom room);

        /// <summary>
        /// 创建房间,用来好友对战....
        /// </summary>
        /// <param name="player"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        IGameRoom CreateRoom(Player player);
        /// <summary>
        /// 移除所有的房间
        /// </summary>
        void RemoveAllRoom();

        /// <summary>
        /// 是否有对应的房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        bool HasRoom(IGameRoom room);

        /// <summary>
        /// 是否有对应的房间
        /// </summary>
        /// <param name="roomId">房间Id</param>
        /// <returns></returns>
        bool HasRoom(int roomId);

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="player"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        bool EnterRoom(Player player, int roomId);
        /// <summary>
        /// 得到所有的房间信息
        /// </summary>
        /// <returns></returns>
        GameRoomInfo[] GetAllRoomInfo();

        bool GetGameRoom(int roomId, out IGameRoom gameRoom);
    }
}
