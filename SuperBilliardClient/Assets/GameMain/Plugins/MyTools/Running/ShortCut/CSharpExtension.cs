using System.Collections.Generic;

namespace Knivt.Tools
{
    public static class CSharpExtension
    {
        public static bool Remove<T>(this Queue<T> queue, T item)
        {
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                T t = queue.Dequeue();
                if (t.Equals(item) == false)
                {
                    queue.Enqueue(item);
                }
            }
            return queue.Count != count;
        }
    }

}
