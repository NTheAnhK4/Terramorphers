using System;
using CoreGame;

namespace Terramorphers.States
{
    public class DeadState : State<TerramorphersEntity>
    {
      
        public DeadState(TerramorphersEntity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.CurrentTile.CurrentOccupant = null;
        }


        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            entity.EntityManager.RemoveEntity(entity);
           
        }
    }
}