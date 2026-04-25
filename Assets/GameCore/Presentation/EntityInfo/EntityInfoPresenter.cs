using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Stats;
using GameCore.Domain.Stats.Icon;
using GameCore.Presentation.Shared;
using UnityEngine;
using VContainer;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.EntityInfo
{
    public class EntityInfoPresenter : ModalPresenter<EntityInfoModal, EntityInfoViewState>
    {
        private Stats _stats;
        [Inject] private IStatIconRepository _statIconRepository;
        [Inject] private TransitionService _transitionService;
        public bool IsClose { get; protected set; }
        public EntityInfoPresenter(EntityInfoModal view, Stats stats) : base(view)
        {
            _stats = stats;
        }

        protected override UniTask Initialize(Memory<object> args, EntityInfoViewState state, EntityInfoModal view)
        {
            var statsTypes = Enum.GetValues(typeof(EStatsType));
            state.OnClose.Subscribe(_ =>OnClose().Forget()).AddTo(view);
            var statIconDatabase = _statIconRepository.Get();
            for (int i = 0; i < statsTypes.Length; ++i)
            {
                EStatsType stat = (EStatsType)statsTypes.GetValue(i);
                int rawValue = _stats.GetRawStatValue(stat);
                int value = _stats.GetStatValue(stat);
                if(rawValue == 0 && value == 0) view.StatViews[i].gameObject.SetActive(false);
                else view.StatViews[i].gameObject.SetActive(true);
                Color amountColor;
                if(rawValue == value) amountColor = Color.white;
                else if(rawValue < value) amountColor = Color.green;
                else amountColor = Color.red;
                string amountFormat;

                if (stat == EStatsType.MaxHP || stat == EStatsType.Mana || stat == EStatsType.Stamina || stat == EStatsType.Range) amountFormat = "{0}";
                else
                {
                    amountFormat = "{0}%";
                    if (value >= rawValue) amountFormat = "+" + amountFormat;
                    else amountFormat = "-" + amountFormat;
                }
                view.StatViews[i].Setup(statIconDatabase.GetByType(stat).Sprite, string.Format(amountFormat, value), amountColor);
            }
            return UniTask.CompletedTask;
        }

        private async UniTask OnClose()
        {
            await _transitionService.ClosePopup();
            IsClose = true;
        }
    }
}