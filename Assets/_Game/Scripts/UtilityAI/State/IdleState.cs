using System;
using System.Collections.Generic;
using CoreGame;
using UnityEngine;
using Random = UnityEngine.Random;
namespace UtilityAI.State
{
   
    public class IdleState : EnemyState<StateData>
    {
        private List<int> _animationHashes;
        private List<List<float>> transitionMatrix;
        private int currentID = 0;
        public IdleState(Enemy entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public IdleState(Enemy entity, int animationHash) : base(entity, animationHash)
        {
        }

        public IdleState(Enemy entity) : base(entity)
        {
        }

        public IdleState(Enemy entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public IdleState(Enemy entity, List<int> animationHashes, List<List<float>> transitionMatix) : base(entity)
        {
            this._animationHashes = animationHashes;
            this.transitionMatrix = transitionMatix;
        } 
        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
          
            currentID = 0;
            entity.Anim.Play(GetAnimHash());
        }
        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            entity.Anim.Play(GetAnimHash());
        }

        private int GetAnimHash()
        {
            float randomValue = Random.value;
            float total = 0;
            for (int i = 0; i < transitionMatrix[currentID].Count; ++i)
            {
                total += transitionMatrix[currentID][i];
                if (randomValue <= total)
                {
                    currentID = i;
                    return _animationHashes[i];
                }
            }

            currentID = 0;
            return _animationHashes[currentID];
        }
    }
}