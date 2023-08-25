using GameMessage;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.Play
{
    /// <summary>
    /// 线程不安全，请放到主线程来调用
    /// </summary>
    public interface IGameManager
    {

        void RigisterGameGroup(IGameGroup gameGroup);

        void MatchGame(Player player, GameType gameType);

        IGameRoom CreateGameRoom(Player player, GameType gameRoom);

        bool EnterRoom(Player player, GameType gameType, int roomid);

        bool RemoveRoom(IGameRoom room);
    }
}
