using UnityEngine;

namespace GameCore.Domain.Skill
{
    public interface ISkillHandler
    {
        void Apply(MonoBehaviour context);
    }
}