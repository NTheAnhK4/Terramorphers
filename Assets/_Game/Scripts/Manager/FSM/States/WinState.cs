using UnityEngine;

namespace Terramorphers
{
    public class WinState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"[Test] win state");
        }
    }
}