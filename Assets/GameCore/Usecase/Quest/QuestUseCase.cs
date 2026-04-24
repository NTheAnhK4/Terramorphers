using System.Collections.Generic;
using GameCore.APIGateway.Quest;
using GameCore.Domain.Quest;

namespace GameCore.Usecase.Quest
{
    public class QuestUseCase
    {
        private IQuestRepository _questRepository;
        private IQuestDatabase _questDatabase;
        private QuestAPIGateway _apiGateway;
        private Dictionary<(EQuestActionType actionType, EQuestTargetType targetType, int targetID), List<QuestMetadata>> toQuestMetadata = new();

        public QuestUseCase(QuestAPIGateway apiGateway, IQuestRepository questRepository)
        {
            _apiGateway = apiGateway;
            _questRepository = questRepository;
            toQuestMetadata = new();
        }

        private void CacheData()
        {
            if (toQuestMetadata.Count == 0)
            {
                toQuestMetadata.Clear();
                if (_questDatabase == null) _questDatabase = _questRepository.Get();
                for (int i = 0; i < _questDatabase.DataCount; ++i)
                {
                    var questMetadata = _questDatabase.GetByType(i);
                    if(questMetadata == null) continue;
                    (EQuestActionType actionType, EQuestTargetType targetType, int targetID) key = (questMetadata.ActionType, questMetadata.TargetType, questMetadata.TargetID);
                    toQuestMetadata.TryAdd(key, new List<QuestMetadata>());
                    toQuestMetadata[key].Add(questMetadata);
                }
            }
        }

        public void AddQuest(QuestMetadata questMetadata)
        {
            CacheData();
            (EQuestActionType actionType, EQuestTargetType targetType, int targetID) key = (questMetadata.ActionType, questMetadata.TargetType, questMetadata.TargetID);
            toQuestMetadata.TryAdd(key, new List<QuestMetadata>());
            toQuestMetadata[key].Add(questMetadata);
        }

        public void RemoveQuest(QuestMetadata questMetadata)
        {
            CacheData();
            (EQuestActionType actionType, EQuestTargetType targetType, int targetID) key = (questMetadata.ActionType, questMetadata.TargetType, questMetadata.TargetID);
            if (toQuestMetadata.TryGetValue(key, out List<QuestMetadata> questMetadatas)) questMetadatas.Remove(questMetadata);
            _apiGateway.RemoveQuest(questMetadata.QuestID);
        }

        public EQuestStatusType GetQuestStatus(int questID) => _apiGateway.GetQuestStatus(questID);
        public void SetQuestStatus(int questID, EQuestStatusType questStatusType) => _apiGateway.SetQuestStatus(questID, questStatusType);
        public int GetQuestProgress(int questID) => _apiGateway.GetQuestProgress(questID);

        public void IncreaseQuestProgress(EQuestActionType actionType, EQuestTargetType targetType, int targetID, int value = 1)
        {
            CacheData();
          
            if (toQuestMetadata.TryGetValue((actionType, targetType, targetID), out List<QuestMetadata> questMetadatas))
            {
                foreach (var quest in questMetadatas)
                {
                    _apiGateway.IncreaseQuestProgress(quest.QuestID,value);
                    EQuestStatusType questStatusType = GetQuestStatus(quest.QuestID);
                    if (questStatusType != EQuestStatusType.InProgress) continue;
                    if (IsQuestFinish(quest))
                    {
                        //DoSomething here
                        // may be notification
                    }
                }
              
            }
            
        }

        public bool IsQuestFinish(QuestMetadata questMetadata)
        {
            int progress = _apiGateway.GetQuestProgress(questMetadata.QuestID);
            int required = questMetadata.RequiredAmount;
            return questMetadata.ComparisonType switch
            {
                EQuestComparisonType.Less => progress < required,
                EQuestComparisonType.LessOrEqual => progress <= required,
                EQuestComparisonType.Equal => progress == required,
                EQuestComparisonType.GreaterOrEqual => progress >= required,
                EQuestComparisonType.Greater => progress > required,
                _ => false
            };
        }
        
        
    }
}