using GameCore.Domain.Quest;
using GameCore.Respository.Shared;

namespace GameCore.Respository.Quest
{
    public class QuestRepository : BaseRepository<int, QuestMetadata, IQuestDatabase>, IQuestRepository
    {
        public override string AddressPath => "QuestDatabase";
    }
}