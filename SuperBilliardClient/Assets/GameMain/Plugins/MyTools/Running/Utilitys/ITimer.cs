
namespace Knivt.Tools
{
    public interface ITimer
    {
        float Elapsed
        {
            get;
        }

        void Reset();
    }
}
