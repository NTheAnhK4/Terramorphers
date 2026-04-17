

using VitalRouter;

namespace GameCore.Commands
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