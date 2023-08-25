using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SuperBilliardServer.Tools.StatePattern
{
    public interface IContext<TOwner>
    {
        void SetStartState<T>() where T : IState<TOwner>, new();

        void Handle(IContext<TOwner> context, TOwner owner);

        internal void ChangeState<T>(IContext<TOwner> context) where T : IState<TOwner>, new();
    }
}
