using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Domain.Level;
using GameCore.Domain.Reward;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Currency;
using GameCore.Usecase.Level;
using GameCore.Usecase.Quest;
using GameCore.Usecase.Skill;
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
        private CurrencyUseCase _currencyUseCase;
        private SkillUseCase _skillUseCase;
        [Inject]
        public void Constructor(TransitionService transitionService, ILevelRepository levelRepository, 
            LevelUseCase levelUseCase, QuestUseCase questUseCase, CurrencyUseCase currencyUseCase, SkillUseCase skillUseCase)
        {
            _transitionService = transitionService;
            _levelRepository = levelRepository;
            _levelUseCase = levelUseCase;
            _questUseCase = questUseCase;
            _currencyUseCase = currencyUseCase;
            _skillUseCase = skillUseCase;
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
            List<StageRewardItem> rewardItems = GetReward(totalStars);
            var winGamePresentor = await _transitionService.ShowWinGameModal(totalStars, rewardItems);
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
                _levelUseCase.SetCurrentStageOfLevel(currentLevelID, currentStageID);
                currentLevelID++;
                if (currentLevelID >= _levelDatabase.DataCount) return;
                currentStageID = 0;
            }

            _levelModel.CurrentLevel = currentLevelID;
            _levelUseCase.SetCurrentStageOfLevel(currentLevelID, currentStageID);
            _levelUseCase.Update(_levelModel).Forget();

        }

        private List<StageRewardItem> GetReward(int stars)
        {
            List<StageRewardItem> result = new();
            LevelMetadata levelMetadata = _levelDatabase.GetByType(_levelModel.SelectedLevel);
            int currentStage = _levelModel.SelectedStage;
            Dictionary<(ERewardItemType, int), int> mergeRewardDict = new();
            foreach (var stageRewardData in levelMetadata.RewardData)
            {
                if (currentStage < stageRewardData.MinStageRewquired) continue;
                int requiredStars = stageRewardData.RequiredStars;
                if(stars < requiredStars) continue;

                int amount = stageRewardData.RewardItemData.GetAmount();
                if(amount == 0) continue;
                (ERewardItemType, int) key = (stageRewardData.RewardItemData.RewardItemType, stageRewardData.RewardItemData.SkillID);
                mergeRewardDict.TryAdd(key, 0);
                mergeRewardDict[key] += amount;
               
                
            }

            foreach (var item in mergeRewardDict)
            {
                if (item.Key.Item1 == ERewardItemType.Coin)
                {
                    var currencyModel = _currencyUseCase.GetModel();
                    _currencyUseCase.AddGold(currencyModel, item.Value);
                }
                else
                {
                    if(_skillUseCase.IsSkillUnlock(item.Key.Item2)) continue;
                    _skillUseCase.UnlockSkill(item.Key.Item2);
                }
                StageRewardItem stageRewardItem = new StageRewardItem()
                {
                    RewardItemType = item.Key.Item1,
                    SkillID = item.Key.Item2,
                    Amount = item.Value
                };
                result.Add(stageRewardItem);
                if (result.Count == 4) return result;
            }

            return result;
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