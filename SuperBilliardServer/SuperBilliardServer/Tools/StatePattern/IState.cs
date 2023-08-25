using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBilliardServer.Tools.StatePattern
{
    public interface IState<TOwner>
    {
        void Handle(IContext<TOwner> context, TOwner owner);
    }
}