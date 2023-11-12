using System.Text;
using GameFramework;

namespace SuperBilliardServer.Debug.MessageStyle
{
    public class TitleStyle : MessageStyleBase
    {
        public override string MessageParser(params string[] messages)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append("====================");
            foreach (var item in messages)
            {
                _stringBuilder.Append(item);
            }
            _stringBuilder.Append("====================");
            return _stringBuilder.ToString();
        }
    }
}