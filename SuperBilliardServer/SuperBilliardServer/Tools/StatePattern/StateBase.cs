namespace SuperBilliardServer.Tools.StatePattern
{
    public abstract class StateBase<TOwner> : IState<TOwner>
    {
        public abstract void Handle(IContext<TOwner> context, TOwner owner);
        protected void ChangeState<T>(IContext<TOwner> context) where T : IState<TOwner>, new()
        {
            context.ChangeState<T>(context);
        }
    }
}
