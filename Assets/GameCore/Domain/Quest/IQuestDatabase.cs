using GameCore.Domain.Shared;

namespace GameCore.Domain.Quest
{
    public interface IQuestDatabase : IMasterDatabase<int, QuestMetadata>
    {
        int DataCount { get; }
    }
}