
namespace SuperBilliardServer.Constant
{
    public static class ServerConstant
    {
        /// <summary>
        /// 断开连接的时间
        /// </summary>
        public const int DisConnnectDataLineTime = 5000;
        /// <summary>
        /// 注册的销毁时间
        /// </summary>
        public const int UnrigisterDeadLineTime = 10000;
        /// <summary>
        /// sql连接字符串
        /// </summary>
        public const string SqlConnectionKey = "Data Source=.;Initial Catalog=ShootingGame;Integrated Security=True";
    }
}
