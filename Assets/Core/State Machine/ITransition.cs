using System;

namespace CoreGame
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
        Func<StateData> Data { get; }
    }
}