using SuperBilliardServer.Debug;
using SuperBilliardServer.Network.Packets;

namespace SuperBilliardServer.Network.User
{
    public class PlayerMessage
    {
        private List<string> _onlineFriendList = new List<string>();
        private Queue<string> _removeFriendQueue = new Queue<string>();

        /// <summary>
        /// 添加在线的好友
        /// </summary>
        /// <param name="username">对应的玩家名称</param>
        public void AddOnlineFriend(string username)
        {
            if (_onlineFriendList.Contains(username) == false)
            {
                _onlineFriendList.Add(username);
            }
        }

        public bool HasOnlineFriend(string username)
        {
            return _onlineFriendList.Contains(username);
        }

        /// <summary>
        /// 发送相应的消息包
        /// </summary>
        /// <param name="packet">消息包</param>
        public void SendPacketToAllOnlineFriend(Packet packet)
        {
            for (int i = 0; i < _onlineFriendList.Count; i++)
            {
                Player player = PlayerManager.Instance.GetPlayer(_onlineFriendList[i]);
                if (player == null)
                {
                    _removeFriendQueue.Enqueue(_onlineFriendList[i]);
                }
                else
                {
                    Log.Debug("更新对方的状态..{0}", player.UserName);
                    player.SendPacket(packet);
                }
            }
            //移除对应的玩家
            while (_removeFriendQueue.Count > 0)
            {
                _onlineFriendList.Remove(_removeFriendQueue.Dequeue());
            }
        }

        /// <summary>
        /// 移除对应的好友
        /// </summary>
        /// <param name="username"></param>
        public void RemoveOnlineFriend(string username)
        {
            _onlineFriendList.Remove(username);
        }
        /// <summary>
        /// 清除一下
        /// </summary>
        public void Clear()
        {
            _onlineFriendList.Clear();
            _removeFriendQueue.Clear();
        }
    }
}
