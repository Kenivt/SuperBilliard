using GameFramework;
using GameMessage;
using SuperBilliardServer.Network.Packets;
using SuperBilliardServer.Network.User;

namespace SuperBilliardServer.Network.Play
{
    public enum GameState
    {
        Playering,
        Waitting,
        End,
    }
    public interface IGameRoom : IReference
    {
        int RoomId { get; }
        GameType GameType { get; }
        GameState GameState { get; set; }
        int PlayerCount { get; }
        PlayerRoomState[] PlayerRoomStates { get; }
        int RoomSeed { get; }

        bool Enter(Player player);

        bool Leave(Player player);

        void ReadyGame(Player player, bool isReady);

        void LoadGameComplete(Player player);
        bool Broadcast(Packet packet);
        bool SendMessageToAnother(Player sendClient, Packet packet);
        bool SendKcpMessageToAnother(Player sendClient, Packet packet);
        void StartLoadGame();
        void EndGame(GameResult gameResult, Player player);
    }
}