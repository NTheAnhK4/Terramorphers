using VitalRouter;
namespace GameCore.Commands
{
    public class ChangePlayerStaminaCommand : ICommand
    {
        public int Stamina { get; set; }
        public int MaxStamina { get; set; }
    }
}