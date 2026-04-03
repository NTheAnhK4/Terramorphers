

using VitalRouter;

namespace GameCore.Commands
{
    public class ChangePlayerManaCommand : ICommand
    {
        public int Mana { get; set; }
        public int MaxMana { get; set; }
    }
}