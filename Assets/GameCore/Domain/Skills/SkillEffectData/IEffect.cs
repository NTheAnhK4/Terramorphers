using GameCore.Domain.Stats;

namespace GameCore.Domain.Skill
{
    public interface IEffect
    {
        void Execute<T>(T owner, EEffectTriggerType trigger);
    }
}