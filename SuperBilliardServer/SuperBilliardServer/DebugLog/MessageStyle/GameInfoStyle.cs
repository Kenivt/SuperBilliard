using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SuperBilliardServer.Debug.MessageStyle
{
    public class GameInfoStyle : MessageStyleBase
    {
        public override string MessageParser(params string[] messages)
        {
            return "GameInfo: " + string.Join(", ", messages);
        }
    }
}
