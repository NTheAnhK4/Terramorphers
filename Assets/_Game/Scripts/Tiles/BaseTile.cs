using System;
using CoreGame;
using Cysharp.Threading.Tasks;
using GameCore.Utility.Shape;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using DG.Tweening;
using GameCore.Domain.Tile;
using GameCore.Presentation.Shared;
using GameCore.Utility;
using R3;
using UnityEngine.Serialization;
using VContainer;

namespace Terramorphers
{
    public abstract class BaseTile : ComponentBehaviour, ITile, IInfoProvider
    {
        [Inject] protected TransitionService _transitionService;
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
            tileTargetSkillColor;

        [FormerlySerializedAs("selftTargetSkillColor")] [SerializeField, TabGroup("Config")] protected Color selfTargetSkillColor;
        private ETileState _currentState;
        protected DisposableBag _bag;
       [TabGroup("Debug"), SerializeField] private Context _context;
       public override void LoadComponent()
       {
           base.LoadComponent();
           if(amountText == null) amountText = transform.Find("Modal/Canvas").GetComponentInChildren<TextMeshProUGUI>();
           if(interactableSR == null) interactableSR = transform.Find("Modal/Interactable").GetComponent<SpriteRenderer>();
           if(tileSkillEffect == null) tileSkillEffect = transform.Find("Modal/Effect").GetComponent<DOTweenAnimation>();
           if (tileSkillSpriteRenderer == null) tileSkillSpriteRenderer = transform.Find("Modal/Effect").GetComponent<SpriteRenderer>();
           ColorUtility.TryParseHtmlString("#0CFF00", out movableColor);
           ColorUtility.TryParseHtmlString("#FFFFFF", out skillApplicableColor);
           ColorUtility.TryParseHtmlString("#FF0000", out enemyTargetSkillColor);
           ColorUtility.TryParseHtmlString("#00FFFB", out allyTargetSkillColor);
           ColorUtility.TryParseHtmlString("#FF0000", out tileTargetSkillColor);
           ColorUtility.TryParseHtmlString("#00FFFB", out selfTargetSkillColor);
           
       }
       


       ETileState ITile.CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }

        protected override void Awake()
        {
            base.Awake();
            IsShowInfo.Skip(1).Subscribe(ShowInfo).AddTo(ref _bag);
            _context = new Context();
        }

        protected void OnDestroy()
        {
            _bag.Dispose();
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
                    interactableSR.color = selfTargetSkillColor;
                    interactableSR.gameObject.SetActive(true);
                    SetSkillTileEffect(selfTargetSkillColor);
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

        public void Init( TileMetadata tileMetadata)
        {
            
            TileMetadata = tileMetadata;
            CurrentOccupant = null;
        }

        public virtual void ApplyEffect(TerramorphersEntity entity)
        {
           
        }

        public virtual void RemoveEffect(TerramorphersEntity entity)
        {
           
        }


        private void SetSkillTileEffect(Color color)
        {
            tileSkillEffect.gameObject.SetActive(true);
            color.a = 1;
            tileSkillSpriteRenderer.color = color;
            tileSkillEffect.DOPlay();
        }

        public ReactiveProperty<bool> IsShowInfo { get; } = new();

        protected void ShowInfo(bool isShow)
        {
            
            if (TileMetadata.Type == ETileType.BasicTile) return;

           
            bool isDisplayRight;
           
            if ((int)(Index.q) + Mathf.FloorToInt((Index.r + 1) / 2) <= 3)
            {
                
                isDisplayRight = false;
            }
            else
            {
               
                isDisplayRight = true;
            }
            
         

            _transitionService.ShowTileView(
                isShow, 
                TileMetadata.TileName, 
                TileMetadata.Description, 
                transform.position,
                isDisplayRight).Forget();
        }
    }
}