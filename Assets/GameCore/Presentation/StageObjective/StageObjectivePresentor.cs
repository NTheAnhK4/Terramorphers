using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Domain.Quest;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;
using GameCore.Usecase.Quest;
using GameCore.Utility;
using VContainer;
using WEngine.MVP;
using R3;


namespace GameCore.Presentation.StageObjective
{
    public class StageObjectivePresentor : ModalPresenter<StageObjectiveModal, StageObjectiveViewState>
    {
        private TransitionService _transitionService;
        private ILevelRepository _levelRepository;
        private QuestUseCase _questUseCase;
        private LevelUseCase _levelUseCase;
        public bool IsClose { get; private set; }


    [Inject]
        public void Constructor(TransitionService transitionService, ILevelRepository levelRepository, 
            LevelUseCase levelUseCase, QuestUseCase questUseCase)
        {
            _transitionService = transitionService;
            _levelRepository = levelRepository;
            _levelUseCase = levelUseCase;
            _questUseCase = questUseCase;
        }
        public StageObjectivePresentor(StageObjectiveModal view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, StageObjectiveViewState state, StageObjectiveModal view)
        {
            IsClose = false;
            ILevelDatabase _levelDatabase = _levelRepository.Get();
            LevelModel levelModel = _levelUseCase.GetModel();
            LevelStageData levelStageData = _levelDatabase
                .GetByType(levelModel.SelectedLevel)
                .LevelStageDatas[levelModel.SelectedStage];
            for (int i = 0; i < levelStageData.StageStarObjectives.Count; ++i)
            {
                var starObjective = levelStageData.StageStarObjectives[i];
                int amount = _questUseCase.GetQuestProgress(starObjective.QuestID);
                state.ObjectiveDescriptions.Add(new ReactiveProperty<string>());
                state.IsObjectiveFailed.Add(new ReactiveProperty<bool>());
                
                state.ObjectiveDescriptions[i].Value = String.Format(starObjective.Description, amount);
                state.IsObjectiveFailed[i].Value = !_questUseCase.IsQuestFinish(starObjective);
            }
            
            state.CloseCommand.Subscribe(OnClose).AddTo(view);
            return UniTask.CompletedTask;
        }

        

        private void OnClose(Unit _)
        {
            IsClose = true;
            _transitionService.ClosePopup();
        }
        
    }
}