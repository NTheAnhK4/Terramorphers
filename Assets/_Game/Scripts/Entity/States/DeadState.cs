using System;
using CoreGame;

namespace Terramorphers.States
{
    public class DeadState : State<TerramorphersEntity>
    {
        public DeadState(TerramorphersEntity entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public DeadState(TerramorphersEntity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public DeadState(TerramorphersEntity entity) : base(entity)
        {
        }

        public DeadState(TerramorphersEntity entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }
    }
}