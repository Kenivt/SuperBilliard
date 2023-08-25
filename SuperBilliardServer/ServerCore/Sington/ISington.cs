
namespace ServerCore.Sington
{
    public interface ISington : IDisposable
    {
        bool IsDisposed { get; }
        void Rigister();
        void ShutDown();
    }
}