using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Presentation.HeroInfo.CurrentSkill;
using GameCore.Presentation.HeroInfo.SkillFrame;
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
        
       

        [Inject]
        public void Constructor(IObjectResolver resolver, ISkillRepository skillRepository, TransitionService transitionService)
        {
            _resolver = resolver;
            _skillRepository = skillRepository;
            _transitionService = transitionService;
        }
        public HeroInfoPresenter(HeroInfoScreen view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, HeroInfoViewState state, HeroInfoScreen view)
        {
            base.Initialize(args, state, view);
            state.ExitCommand.Subscribe(OnExit).AddTo(view);
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
            return UniTask.CompletedTask;
        }

        private void OnExit(Unit _)
        {
            _transitionService.ShowLobbyScreen().Forget();
        }
    }
}