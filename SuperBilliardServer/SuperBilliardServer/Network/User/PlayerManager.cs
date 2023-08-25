using GameMessage;
using ServerCore.Sington;
using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.User
{
    /// <summary>
    /// 请放到主线程来调用,线程不安全
    /// </summary>
    public class PlayerManager : SingtonBase<PlayerManager>
    {
        public readonly Dictionary<string, Player> players = new Dictionary<string, Player>();

        public void RigisterPlayer(string username, Client client)
        {
            if (players.ContainsKey(username))
            {
                Log.Error("已经存在对应的玩家了...");
            }
            else
            {
                Player player = new Player(client, username);
                client.Player = player;
                players.Add(username, player);
            }
        }

        public void UnrigisterPlayer(string username)
        {
            if (players.ContainsKey(username))
            {
                players.Remove(username);
            }
            else
            {
                Log.Error("不存在对应的玩家");
            }
        }

        public PlayerStatus GetPlayerStatus(string username)
        {
            if (players.ContainsKey(username))
            {
                return players[username].State;
            }
            else
            {
                Log.Error("不存在对应的玩家");
                return PlayerStatus.Idle;
            }
        }
        public Player GetPlayer(string username)
        {
            if (players.ContainsKey(username))
            {
                return players[username];
            }
            else
            {
                //Log.Error("不存在对应的玩家");
                return null;
            }
        }
        public void SendMessageToAnotherPlayer(string username, Packet packet)
        {
            if (players.ContainsKey(username))
            {
                players[username].SendPacket(packet);
            }
            else
            {
                Log.Error("不存在对应的玩家");
            }
        }
    }
}