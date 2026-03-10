using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terramorphers
{
    public class AdvantureState : GameState
    {
        private EntityManager _entityManager; 
        public AdvantureState(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            _entityManager.OnEnter();
            Debug.Log($"[Test] advanture ");
        }

        public override void OnUpdate()
        {
            _entityManager.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _entityManager.OnExit();
        }
    }

}
