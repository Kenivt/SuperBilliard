using GameFramework;

namespace SuperBilliardServer.Debug.MessageStyle
{
    public class ShowReferenceInfo : MessageStyleBase
    {
        public override string MessageParser(params string[] messages)
        {
            ReferencePoolInfo[] referencePoolInfos = ReferencePool.GetAllReferencePoolInfos();
            _stringBuilder.Clear();
            int count = 0;
            int max = 5;
            foreach (var item in referencePoolInfos)
            {
                if ((count++) % max == 0)
                {
                    _stringBuilder.AppendLine("ReferencePoolInfo:");
                    _stringBuilder.AppendLine($"-----------------------------------------------------------------------------------------------------------------------");
                    _stringBuilder.AppendLine($" | UnusedReferenceCount | UsingReferenceCount | AcquireReferenceCount | ReleaseReferenceCount | RemoveReferenceCount |");
                    _stringBuilder.AppendLine($"-----------------------------------------------------------------------------------------------------------------------");
                }
                else
                {
                    _stringBuilder.AppendLine(new string('-', 110));
                }
                _stringBuilder.AppendLine($"--***---ReferenceItemType:{item.Type.FullName}---***--");
                _stringBuilder.Append(" | ");
                _stringBuilder.Append($"{item.UnusedReferenceCount,-20}");
                _stringBuilder.Append(" | ");
                _stringBuilder.Append($"{item.UsingReferenceCount,-19}");
                _stringBuilder.Append(" | ");
                _stringBuilder.Append($"{item.AcquireReferenceCount,-21}");
                _stringBuilder.Append(" | ");
                _stringBuilder.Append($"{item.ReleaseReferenceCount,-21}");
                _stringBuilder.Append(" | ");
                _stringBuilder.Append($"{item.RemoveReferenceCount,-20}");
                _stringBuilder.Append(" | ");
                _stringBuilder.AppendLine();
            }
            return _stringBuilder.ToString();
        }
    }
}
