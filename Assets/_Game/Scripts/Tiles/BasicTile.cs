using CoreGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Terramorphers
{
    public class BasicTile : ComponentBehaviour, ITile
    {
        [SerializeField, TabGroup("Components")]
        protected SpriteRenderer interactableSR;

        [SerializeField, TabGroup("Config")] protected Color movableColor, skillApplicableColor;
        private ETileState _currentState;

        ETileState ITile.CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        public void ChangeState(ETileState newState)
        {
            _currentState = newState;
            switch (_currentState)
            {
                case ETileState.Normal:
                    interactableSR.gameObject.SetActive(false);
                    break;
                case ETileState.Movable:
                    interactableSR.color = movableColor;
                    interactableSR.gameObject.SetActive(true);
                    break;
                case ETileState.SkillApplicable:
                    interactableSR.color = skillApplicableColor;
                    interactableSR.gameObject.SetActive(true);
                    break;
            }
        }
    }
}