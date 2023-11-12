using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.Sington
{
    public abstract class SingtonBase<T> : ISington where T : SingtonBase<T>, new()
    {
        public static T Instance
        {
            get;
            private set;
        }
        public bool IsDisposed
        {
            get;
            private set;
        }

        public void Rigister()
        {
            if (Instance != null)
            {
                return;
            }
            IsDisposed = false;
            Instance = (T)this;
        }

        public void ShutDown()
        {
            if (Instance == null)
            {
                return;
            }
            IsDisposed = true;
            Instance.Dispose();
            Instance = null;
        }

        //移除所有的任务
        public virtual void Dispose()
        {
        }
    }
}
