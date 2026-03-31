using GameCore.Domain.Skill;
using VContainer;

namespace Terramorphers
{
    public class SkillManager
    {
        #region Variables

        private ISkillRepository _skillRepository;
        private ISkillDatabase cacheSkillDatabase;

        private ISkillDatabase _skillDatabase
        {
            get
            {
                if (cacheSkillDatabase == null) cacheSkillDatabase = _skillRepository.Get();
                return cacheSkillDatabase;
            }
        }

        public SkillMetadata GetSkillMetadata(int skillId) => _skillDatabase.GetByType(skillId);

        #endregion
        

        [Inject]
        public void Constructor(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        
    } 
}