


using System;
using UnityEngine;
namespace CoreGame
{
    public interface IEntity
    {
       
       
       
    }

    public class Entity : ComponentBehaviour, IEntity
    {
        public string curentState;
        public bool IsAnimationTriggerFinished;
        public Animator Anim;

    }
    
}