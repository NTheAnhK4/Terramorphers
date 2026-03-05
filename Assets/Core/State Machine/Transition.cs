using System;

namespace CoreGame
{
    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
        public Func<StateData> Data { get; }
        

        public Transition(IState to, IPredicate condition, Func<StateData> getData = null)
        {
            To = to;
            Condition = condition;
            Data = getData;
        }
    }
}