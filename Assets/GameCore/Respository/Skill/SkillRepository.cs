using GameCore.Domain.Skill;
using GameCore.Respository.Shared;

namespace GameCore.Respository.Skill
{
    public class SkillRepository : BaseRepository<int, SkillMetadata, ISkillDatabase>, ISkillRepository
    {
        public override string AddressPath => "SkillDatabase";
    }
}