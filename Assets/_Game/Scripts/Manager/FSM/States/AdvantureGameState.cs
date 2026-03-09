using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terramorphers
{
    public class AdvantureGameState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"[Test] advanture ");
        }
    }

}
