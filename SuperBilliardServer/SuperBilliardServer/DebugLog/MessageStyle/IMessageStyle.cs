using GameFramework;

namespace SuperBilliardServer.Debug.MessageStyle
{
    /// <summary>
    /// 写的很差,不好用
    /// </summary>
    public interface IMessageStyle : IReference
    {
        string MessageParser(params string[] messages);
    }
}
