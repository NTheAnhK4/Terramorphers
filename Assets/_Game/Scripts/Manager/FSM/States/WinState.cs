using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;
using GameCore.Usecase.Quest;
using GameCore.Utility.Audio.GameAudio;
using JSAM;
using UnityEngine;
using VContainer;

namespace Terramorphers
{
    public class WinState : GameState
    {
        private TransitionService _transitionService;
        private ILevelRepository _levelRepository;
        
        private LevelUseCase _levelUseCase;
        private QuestUseCase _questUseCase;
        private LevelModel _levelModel;
        private ILevelDatabase _levelDatabase;
        [Inject]
        public void Constructor(TransitionService transitionService, ILevelRepository levelRepository, 
            LevelUseCase levelUseCase, QuestUseCase questUseCase)
        {
            _transitionService = transitionService;
            _levelRepository = levelRepository;
            _levelUseCase = levelUseCase;
            _questUseCase = questUseCase;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            _levelModel = _levelUseCase.GetModel();
            _levelDatabase = _levelRepository.Get();
            
            UnlockNextLevel();
            PlayMusic();
            EnterAsync().Forget();
        }

        private async UniTask EnterAsync()
        {
            int totalStars = GetStars();
           
            var winGamePresentor = await _transitionService.ShowWinGameModal(totalStars);
        }
        private void PlayMusic()
        {
            var audio = AudioManager.PlayMusic(EMusicType.VictoryMusic);
            if (!AudioManager.MusicMuted)
            {
                audio.AudioSource.volume = 0;
                audio.AudioSource.DOFade(1, .15f);
            }
        }


        private int GetStars()
        {
            var levelStageData = _levelDatabase
                .GetByType(_levelModel.SelectedLevel)
                .LevelStageDatas[_levelModel.SelectedStage];
            int totalStars = 0;
            foreach (var questMetadata in levelStageData.StageStarObjectives)
            {
                bool isCompleted = _questUseCase.IsQuestFinish(questMetadata);
                if (isCompleted) totalStars++;
                _questUseCase.RemoveQuest(questMetadata);
            }
            _levelUseCase.SetStars(_levelModel.SelectedLevel, _levelModel.SelectedStage, totalStars);
            return totalStars;
        }

        private void UnlockNextLevel()
        {
            int currentLevelID = _levelModel.SelectedLevel;
            int currentStageID = _levelModel.SelectedStage + 1;
            var levelMetaData = _levelDatabase.GetByType(currentLevelID);

            if (currentStageID >= levelMetaData.LevelStageDatas.Count)
            {
                currentLevelID++;
                if (currentLevelID >= _levelDatabase.DataCount) return;
                currentStageID = 0;
            }

            _levelModel.CurrentLevel = currentLevelID;
            _levelUseCase.SetCurrentStageOfLevel(currentLevelID, currentStageID);
            _levelUseCase.Update(_levelModel).Forget();

        }

        public override void OnExit()
        {
            base.OnExit();
            if (AudioManager.TryGetPlayingMusic(EMusicType.VictoryMusic, out MusicChannelHelper audio))
            {
                audio.AudioSource.DOFade(0, 0.5f)
                    .OnComplete(() =>
                    {
                        AudioManager.StopMusic(EMusicType.VictoryMusic, stopInstantly: true);
                    });
            }
        }
    }
}