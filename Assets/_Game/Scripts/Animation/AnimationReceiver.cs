using System.Collections;
using System.Collections.Generic;
using CoreGame;
using UnityEngine;

namespace Terramorphers
{
    public class AnimationReceiver : ComponentBehaviour
    {
        [SerializeField] private Entity entity = null;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (entity == null) entity = GetComponentInParent<Entity>();
        }

        public void AnimationFinishTrigger()
        {
            if(entity != null) entity.AnimationFinishTrigger();
        }

        public void AnimationTrigger()
        {
            if(entity != null) entity.AnimationTrigger();
        }
    }

}
