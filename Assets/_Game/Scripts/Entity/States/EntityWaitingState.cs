using System;
using CoreGame;

namespace Terramorphers.States
{
    public class EntityWaitingState : State<Entity>
    {
        public EntityWaitingState(Entity entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public EntityWaitingState(Entity entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }
    }
}