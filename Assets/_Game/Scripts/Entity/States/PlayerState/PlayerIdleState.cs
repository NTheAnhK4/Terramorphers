using System;
using CoreGame;

namespace Terramorphers.States.PlayerState
{
    public class PlayerIdleState : State<Player>
    {
        public PlayerIdleState(Player entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public PlayerIdleState(Player entity, int animationHash) : base(entity, animationHash)
        {
        }

        public PlayerIdleState(Player entity) : base(entity)
        {
        }

        public PlayerIdleState(Player entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }
    }
}