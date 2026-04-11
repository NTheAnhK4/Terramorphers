using R3;

namespace UtilityAI.DataCache
{
    public class EntityDataCache
    {
        public ReactiveProperty<int> RemainMana { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RemainStamina { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> RemainHP { get; } = new ReactiveProperty<int>();
    }
}