using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SuperBilliardServer.Tools.StatePattern
{
    public sealed class Context<TOwner> : IContext<TOwner>
    {
        public TOwner Owner { get; private set; }

        private readonly Dictionary<Type, IState<TOwner>> _contextMap = new Dictionary<Type, IState<TOwner>>();

        private IState<TOwner>? _currentState;

        public Context(TOwner owner)
        {
            Owner = owner;
        }
        public void ChangeState<T>(IContext<TOwner> context) where T : IState<TOwner>, new()
        {
            Type type = typeof(T);
            if (_contextMap.ContainsKey(type))
            {
                _currentState = _contextMap[type];
            }
            else
            {
                throw new Exception("State not found");
            }
        }

        public void SetStartState<T>() where T : IState<TOwner>, new()
        {
            Type type = typeof(T);
            if (_contextMap.ContainsKey(type))
            {
                _currentState = _contextMap[type];
            }
            else
            {
                throw new Exception("State not found");
            }
        }

        public void Handle(IContext<TOwner> context, TOwner owner)
        {
            if (_currentState == null)
            {
                throw new Exception("State not found");
            }
            _currentState.Handle(context, owner);
        }
    }
}
