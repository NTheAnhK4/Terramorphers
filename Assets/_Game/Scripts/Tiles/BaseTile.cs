using CoreGame;
using GameCore.Utility.Shape;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using DG.Tweening;
using GameCore.Utility;
using UnityEngine.UI;
using UtilityAI;

namespace Terramorphers
{
    public abstract class BaseTile : ComponentBehaviour, ITile
    {
        [SerializeField, TabGroup("Components")]
        private TextMeshProUGUI amountText;

        [SerializeField, TabGroup("Components")]
        protected SpriteRenderer interactableSR;

        [SerializeField, TabGroup("Components")]
        private DOTweenAnimation tileSkillEffect;

        [SerializeField, TabGroup("Components")]
        private SpriteRenderer tileSkillSpriteRenderer;

        [SerializeField, TabGroup("Config")] protected Color movableColor, 
            skillApplicableColor, enemyTargetSkillColor, allyTargetSkillColor, 
            tileTargetSkillColor, selftTargetSkillColor;
        private ETileState _currentState;
       [TabGroup("Debug"), SerializeField] private Context _context;
      


        ETileState ITile.CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        protected override void Awake()
        {
            base.Awake();
            _context = new Context();
        }

        protected virtual void OnEnable()
        {
            ChangeState(ETileState.Normal);
        }

        public void ChangeState(ETileState newState, int cost = 0)
        {
            _currentState = newState;
           
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
                    SetSkillTileEffect(enemyTargetSkillColor);
                    break;
                case ETileState.TileTargetSkill:
                    interactableSR.color = tileTargetSkillColor;
                    interactableSR.gameObject.SetActive(true);
                    SetSkillTileEffect(tileTargetSkillColor);
                    break;
                case ETileState.AllyTargetSkill:
                    interactableSR.color = allyTargetSkillColor;
                    interactableSR.gameObject.SetActive(true);
                    SetSkillTileEffect(allyTargetSkillColor);
                    break;
                case ETileState.SelfTargetSkill:
                    interactableSR.color = selftTargetSkillColor;
                    interactableSR.gameObject.SetActive(true);
                    SetSkillTileEffect(selftTargetSkillColor);
                    break;
            }
        }

        public abstract bool IsPassable();

        public abstract bool IsBlockVisibility();


        public Transform Transform => transform;
        public abstract int GetMoveCost();
        public Cube Index { get; set; }
        public TerramorphersEntity CurrentOccupant { get; set; } = null;
        public TileMetadata TileMetadata { get; set; }

        Context ITile.Context
        {
            get => _context;
            set => _context = value;
        }

       
       


        private void SetSkillTileEffect(Color color)
        {
            tileSkillEffect.gameObject.SetActive(true);
            color.a = 1;
            tileSkillSpriteRenderer.color = color;
            tileSkillEffect.DOPlay();
        }
    }
}