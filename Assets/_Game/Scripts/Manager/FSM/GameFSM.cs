using System.Collections.Generic;

using VContainer.Unity;
using VitalRouter;

namespace Terramorphers
{
	[Routes]
	public partial class GameFSM : ITickable, IFixedTickable, ILateTickable
	{
		private readonly Dictionary<EGameStateType, GameState> _states;
		private GameState _currentState;

		public GameState CurrentState => _currentState;

		public GameFSM(GameManager gameManager, AdvantureGameState advantureGameState, WinState winState )
		{
			_states = new()
			{
				[EGameStateType.AdvantureMode] = advantureGameState,
				[EGameStateType.WinState] = winState
			};
			gameManager.GameFSM = this;
		}

		[Route]
		public void On(ChangeGameStateTypeCommand command)
		{
			ChangeState(command.StateType);
		}

		private void ChangeState(EGameStateType stateType)
		{
			if (_currentState == _states[stateType]) return;
			_currentState?.OnExit();
			_currentState = _states[stateType];
			_currentState?.OnEnter();
		}

		public void Tick()
	    {
		    _currentState?.OnUpdate();
	    }

	    public void FixedTick() => _currentState?.OnFixedUpdate();

	    public void LateTick() => _currentState?.OnLateUpdate();
	}
}