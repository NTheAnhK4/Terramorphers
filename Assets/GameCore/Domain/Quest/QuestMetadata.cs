using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Domain.Quest
{
    [Serializable]
    public class QuestMetadata
    {
        [HorizontalGroup("Row1")]
        [LabelText("Action")]
        [SerializeField] private EQuestActionType actionType;

        [HorizontalGroup("Row1")]
        [LabelText("Target")]
        [SerializeField] private EQuestTargetType targetType;

        [HorizontalGroup("Row2")]
        [LabelText("Compare")]
        [SerializeField] private EQuestComparisonType comparisonType;

        [HorizontalGroup("Row2")]
        [LabelText("Required")]
        [SerializeField] private int requiredAmount;

        [HorizontalGroup("Row3")]
        [LabelText("Target ID")]
        [SerializeField] private int targetID;

        [HorizontalGroup("Row3")]
        [LabelText("Quest ID")]
        [SerializeField] private int questID;

        [SerializeField] private string description;
        
        public EQuestActionType ActionType => actionType;
        public EQuestTargetType TargetType => targetType;
        public EQuestComparisonType ComparisonType => comparisonType;
        public int TargetID => targetID;
        public int RequiredAmount => requiredAmount;

        public int QuestID
        {
            get => questID;
            set => questID = value;
        }

        public string Description => description;
    }
}