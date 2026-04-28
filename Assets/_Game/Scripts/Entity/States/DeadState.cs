using System;
using CoreGame;
using JSAM;
using UnityEngine;

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
            if (entity.EntityMetadata.DeadSound != null) AudioManager.PlaySound(entity.EntityMetadata.DeadSound);
            entity.CurrentTile.CurrentOccupant = null;
        }


        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            entity.EntityManager.RemoveEntity(entity);
           
        }
    }
}