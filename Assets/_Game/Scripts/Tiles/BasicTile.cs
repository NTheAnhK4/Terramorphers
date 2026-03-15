using System;
using CoreGame;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
namespace Terramorphers
{
    public class BasicTile : ComponentBehaviour, ITile
    {
        [SerializeField, TabGroup("Components")]
        private TextMeshProUGUI amountText;
        
        [SerializeField, TabGroup("Components")]
        protected SpriteRenderer interactableSR;

        [SerializeField, TabGroup("Config")] protected Color movableColor, skillApplicableColor;
        private ETileState _currentState;

        ETileState ITile.CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        protected void OnEnable()
        {
            ChangeState(ETileState.Normal);
        }

        public void ChangeState(ETileState newState, int cost = 0)
        {
            _currentState = newState;
            switch (_currentState)
            {
                case ETileState.Normal:
                    interactableSR.gameObject.SetActive(false);
                    amountText.gameObject.SetActive(false);
                    break;
                case ETileState.Movable:
                    interactableSR.color = movableColor;
                    interactableSR.gameObject.SetActive(true);
                    amountText.text = cost.ToString();
                    amountText.gameObject.SetActive(true);
                    break;
                case ETileState.SkillApplicable:
                    interactableSR.color = skillApplicableColor;
                    interactableSR.gameObject.SetActive(true);
                    amountText.text = cost.ToString();
                    amountText.gameObject.SetActive(true);
                    break;
            }
        }

        public bool IsPassable() => true;

        public bool IsBlockVisibility() => false;
        public Transform Transform => transform;
        public int GetMoveCost() => 1;
    }
}