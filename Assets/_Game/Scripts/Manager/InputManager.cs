
using UnityEngine;

namespace Terramorphers
{
    public class InputManager
    {
        enum InputState
        {
            None,
            Move,
            Skill
        }

        private InputState currentState;

        public void OnEnter()
        {
            currentState = InputState.Move;
        }

        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (currentState)
                {
                    case InputState.None:
                        return;
                    case InputState.Move:
                        HandleMove();
                        break;
                    case InputState.Skill:
                        HandleSkill();
                        break;
                }
            }
        }
        

        ITile GetTile()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == null) return null;
            return hit.collider.gameObject.GetComponentInParent<ITile>();
            
        }
        private void HandleMove()
        {
            var tile = GetTile();
            if(tile == null) return;
            tile.ChangeState(ETileState.Movable);
            Debug.Log($"[Test] handle move");
        }

        private void HandleSkill()
        {
            
        }

       
    }
}

