using R3;
namespace Terramorphers
{
    public class AdvantureState : GameState
    {
        private EntityManager _entityManager;
        private InputManager _inputManager;
        private GameManager _gameManager;
        private DisposableBag _bag;
        public AdvantureState(EntityManager entityManager, InputManager inputManager,
            GameManager gameManager)
        {
            _entityManager = entityManager;
            _inputManager = inputManager;
            _gameManager = gameManager;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _gameManager.GamePlayPresenter.IsShowingUI.Subscribe(_inputManager.StopInput).AddTo(ref _bag);
            _entityManager.OnEnter();
        }
       

        public override void OnUpdate()
        {
            _entityManager.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _bag.Dispose();
            _entityManager.OnExit();
        }
    }

}
