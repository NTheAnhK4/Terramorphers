using GameCore.Domain.Shared;
using GameCore.Domain.Skill;
using UnityEngine;

namespace GameCore.Respository.Skill
{
    [CreateAssetMenu(fileName = "SkillDatabase", menuName = "Database/SkillDatabase")]
    public class SkillDatabase : BaseDatabase<int, SkillMetadata>, ISkillDatabase
    {
        public int DataCount => Datas.Count;
    }
}