

using VitalRouter;

namespace Terramorphers
{
    public class ChangeGameStateTypeCommand : ICommand
    {
        public EGameStateType StateType { get; }

        public ChangeGameStateTypeCommand(EGameStateType stateType)
        {
            StateType = stateType;
        }
    }
}