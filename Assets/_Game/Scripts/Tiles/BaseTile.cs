using CoreGame;
using GameCore.Utility.Shape;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Terramorphers
{
    public abstract class BaseTile : ComponentBehaviour, ITile
    {
        [SerializeField, TabGroup("Components")]
        private TextMeshProUGUI amountText;

        [SerializeField, TabGroup("Components")]
        protected SpriteRenderer interactableSR;

        [SerializeField, TabGroup("Config")] protected Color movableColor, skillApplicableColor, enemyTargetSkillColor, allyTargetSkillColor, tileTargetSkillColor;
        private ETileState _currentState;


        ETileState ITile.CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        protected virtual void OnEnable()
        {
            ChangeState(ETileState.Normal);
        }

        public void ChangeState(ETileState newState, int cost = 0)
        {
            _currentState = newState;
            // Mặc định tắt cả
            interactableSR.gameObject.SetActive(false);
            amountText.gameObject.SetActive(false);

            switch (_currentState)
            {
                case ETileState.Movable:
                    interactableSR.color = movableColor;
                    interactableSR.gameObject.SetActive(true);
                    amountText.text = cost.ToString();
                    amountText.gameObject.SetActive(true);
                    break;

                case ETileState.SkillApplicable:
                    interactableSR.color = skillApplicableColor;
                    interactableSR.gameObject.SetActive(true);
                    break;

                case ETileState.EnemyTargetSkill:
                    interactableSR.color = enemyTargetSkillColor;
                    interactableSR.gameObject.SetActive(true);
                    break;
                case ETileState.TileTargetSkill:
                    interactableSR.color = tileTargetSkillColor;
                    interactableSR.gameObject.SetActive(true);
                    break;
                case ETileState.AllyTargetSkill:
                    interactableSR.color = allyTargetSkillColor;
                    interactableSR.gameObject.SetActive(true);
                    break;
            }
        }

        public abstract bool IsPassable();

        public abstract bool IsBlockVisibility();


        public Transform Transform => transform;
        public abstract int GetMoveCost();
        public Cube Index { get; set; }
        public TerramorphersEntity CurrentOccupant { get; set; } = null;
    }
}