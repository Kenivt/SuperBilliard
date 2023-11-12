using System.Text;

namespace SuperBilliardServer.Debug.MessageStyle
{
    public class TableStyle : MessageStyleBase
    {
        public override string MessageParser(params string[] messages)
        {
            int count = messages.Length;
            _stringBuilder.Clear();
            // 获取列数和每列的最大宽度
            int columnCount = messages[0].Split(',').Length;
            int[] maxLengths = new int[columnCount];
            for (int i = 0; i < count; i++)
            {
                string[] values = messages[i].Split(',');
                for (int j = 0; j < columnCount; j++)
                {
                    maxLengths[j] = Math.Max(maxLengths[j], values[j].Length);
                }
            }
            _stringBuilder.Append("+");
            // 绘制表格
            for (int i = 0; i < columnCount; i++)
            {
                _stringBuilder.Append(new string('-', maxLengths[i] + 2) + "+");
            }
            _stringBuilder.AppendLine();
            for (int i = 0; i < count; i++)
            {
                string[] values = messages[i].Split(',');
                _stringBuilder.Append("| ");
                for (int j = 0; j < columnCount; j++)
                {
                    _stringBuilder.Append(values[j].PadRight(maxLengths[j] + 1));
                    _stringBuilder.Append("| ");
                }
                _stringBuilder.AppendLine();
                if (i == 0)
                {
                    _stringBuilder.Append("+");
                    for (int j = 0; j < columnCount; j++)
                    {
                        _stringBuilder.Append(new string('-', maxLengths[j] + 2) + "+");
                    }
                    _stringBuilder.AppendLine();
                }
            }
            _stringBuilder.Append("+");
            for (int i = 0; i < columnCount; i++)
            {
                _stringBuilder.Append(new string('-', maxLengths[i] + 2) + "+");
            }
            _stringBuilder.AppendLine();

            return _stringBuilder.ToString();
        }
    }
}
