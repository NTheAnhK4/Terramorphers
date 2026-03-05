using System;
using System.Collections;
using System.Collections.Generic;
using CoreGame;
using UnityEngine;

namespace Terramorphers
{
    public class InputManager : Singleton<InputManager>
    {
        enum InputState
        {
            None,
            Move,
            Skill
        }

        private InputState currentState;

        private void Start()
        {
            currentState = InputState.Move;
        }

        private void Update()
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

