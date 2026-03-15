using VitalRouter;

namespace GameCore.Commands
{
    public class ToggleEndTurnCommand : ICommand
    {
        public bool IsOn { get; set; }
    }
}