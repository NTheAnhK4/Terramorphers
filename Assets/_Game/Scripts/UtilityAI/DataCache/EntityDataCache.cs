using System.Collections.Generic;
using R3;

namespace UtilityAI.DataCache
{
    public class EntityDataCache
    {
        public ReactiveProperty<int> RemainMana { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RemainStamina { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RemainHP { get; } = new ReactiveProperty<int>();
        public Dictionary<int, ReactiveProperty<int>> SkillCoolDown = new ();
    }
}