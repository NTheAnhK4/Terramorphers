using System.Collections.Generic;
using GameCore.Domain.Quest;
using UnityEngine;

namespace GameCore.APIGateway.Quest
{
    public class QuestAPIGateway
    {
        private Dictionary<int, int> questProgress = new();

       

        public int GetQuestProgress(int questID)
        {
            if (!questProgress.ContainsKey(questID))
            {
                questProgress[questID] = PlayerPrefs.GetInt($"quest_{questID}_progress", 0);
            }

            return questProgress[questID];
        }

        public void RemoveQuest(int questID)
        {
            questProgress.Remove(questID);
        }
        public void IncreaseQuestProgress(int questID, int amount = 1)
        {
            int questAmount = GetQuestProgress(questID) + amount;
            questProgress[questID] = questAmount;
            //currently don't save if questID less than 0
            if (questID < 0) return;
            PlayerPrefs.SetInt($"quest_{questID}_progress", amount);
        }

        public EQuestStatusType GetQuestStatus(int questID)
        {
            int id = PlayerPrefs.GetInt($"quest_{questID}_status", 0);
            return (EQuestStatusType)(id);
        }

        public void SetQuestStatus(int questID,EQuestStatusType questStatusType)
        {
            PlayerPrefs.SetInt($"quest_{questID}_status", (int)(questStatusType));
        }
    }
}