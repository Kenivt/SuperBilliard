using System.Text;

namespace SuperBilliardServer.Debug.MessageStyle
{
    public abstract class MessageStyleBase : IMessageStyle
    {
        protected StringBuilder _stringBuilder = new StringBuilder();
        public virtual void Clear()
        {
            _stringBuilder.Clear();
        }
        public abstract string MessageParser(params string[] messages);
    }
}
