using VitalRouter;

namespace GameCore.Commands
{
    public class IncreaseRoundCommand : ICommand
    {
        public int NewRound { get; set; }
    }
}