using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Presentation.Currency.Gold;
using GameCore.Presentation.HeroInfo.CurrentSkill;
using GameCore.Presentation.HeroInfo.SkillFrame;
using GameCore.Presentation.Panel;
using GameCore.Presentation.Shared;
using GameCore.Presentation.Skill;
using R3;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo
{
    public class HeroInfoPresenter : ScreenPresenter<HeroInfoScreen, HeroInfoViewState>
    {
        private IObjectResolver _resolver;
        private ISkillRepository _skillRepository;
        private TransitionService _transitionService;
        private PanelActivityPresenter _panelActivityPresenter;
       

        [Inject]
        public void Constructor(IObjectResolver resolver, ISkillRepository skillRepository, TransitionService transitionService)
        {
            _resolver = resolver;
            _skillRepository = skillRepository;
            _transitionService = transitionService;
        }
        public HeroInfoPresenter(HeroInfoScreen view, PanelActivityPresenter panelPresenter) : base(view)
        {
            _panelActivityPresenter = panelPresenter;
        }

        protected override async UniTask Initialize(Memory<object> args, HeroInfoViewState state, HeroInfoScreen view)
        {
            base.Initialize(args, state, view);
            state.ExitCommand.Subscribe(_ =>OnExit().Forget()).AddTo(view);
            var skillInfoPresenter = new SkillInfoPresenter(view.SkillInfoView);
            _resolver.Inject(skillInfoPresenter);
            skillInfoPresenter.Initialize();

            ISkillDatabase skillDatabase = _skillRepository.Get();
            List<SkillFramePresenter> skillFramePresenters = new();
            for (int i = 0; i < skillDatabase.DataCount; ++i)
            {
                int skillID = i;
                SkillMetadata skillMetadata = skillDatabase.GetByType(i);
                SkillFrameView skillFrameView = view.AddSkillFrameView();

                SkillFramePresenter presenter = new SkillFramePresenter(
                    skillFrameView, skillMetadata,state.PreviewSkillCommand, skillID, skillInfoPresenter,
                    state.SelectSkillCommand, state.UnselectSkillCommand);
                _resolver.Inject(presenter);
                presenter.Initialize();
            }

            CurrentSkillPresenter currentSkillPresenter = new CurrentSkillPresenter(
                view.CurrentSkillView, state.PreviewSkillCommand, state.HandleSelectionCommand,
                state.SelectSkillCommand, state.UnselectSkillCommand);
            _resolver.Inject(currentSkillPresenter);
            currentSkillPresenter.Initialize();


            state.HidePanelCommand.Subscribe(_ =>HidePanel().Forget()).AddTo(view);

            var goldPresenter = new GoldPresenter(view.GoldView);
            _resolver.Inject(goldPresenter);
            goldPresenter.Initialize();

        }

        private async UniTask HidePanel()
        {
            if (_panelActivityPresenter == null) return;
             await _panelActivityPresenter.HidePanel();
            await _transitionService.ShowPanelActivity(false);
        }

        private async UniTask OnExit()
        {
            var panelPresenter = await _transitionService.ShowPanelActivity(true);
            await panelPresenter.ShowPanel();
            _transitionService.ShowLobbyScreen(panelPresenter).Forget();
        }
    }
}