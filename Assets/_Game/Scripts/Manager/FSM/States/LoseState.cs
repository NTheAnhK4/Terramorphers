using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using VContainer;

namespace Terramorphers
{
    public class LoseState : GameState
    {
        private TransitionService _transitionService;

        [Inject]
        public void Constructor(TransitionService transitionService)
        {
            _transitionService = transitionService;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            EnterAsync().Forget();
        }

        private async UniTask EnterAsync()
        {
            var presentor = await _transitionService.ShowLoseGameModal();
        }
    }
}