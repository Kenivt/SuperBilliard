using ServerCore.Sington;
using System.Collections.Concurrent;

namespace ServerCore
{
    public class ThreadSynchronizationContext : SynchronizationContext
    {
        public readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        public void Update()
        {
            while (true)
            {
                if (!_actions.TryDequeue(out Action action))
                {
                    return;
                }

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        //同步上下文的机制，设定此同步类才可以
        public override void Post(SendOrPostCallback callback, object state)
        {
            this.Post(() => callback(state));
        }

        public void Post(Action action)
        {
            _actions.Enqueue(action);
        }
    }

    public class MainThreadSyncContext : SingtonBase<MainThreadSyncContext>, IUpdateSington
    {
        public readonly static ThreadSynchronizationContext _context = new ThreadSynchronizationContext();

        public MainThreadSyncContext()
        {
            SynchronizationContext.SetSynchronizationContext(_context);
        }

        public void Post(SendOrPostCallback callback, object state)
        {
            this.Post(() => callback(state));
        }

        public void Update()
        {
            _context.Update();
        }

        public void Post(Action action)
        {
            _context.Post(action);
        }
    }
}