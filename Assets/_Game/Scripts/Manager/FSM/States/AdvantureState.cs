

namespace Terramorphers
{
    public class AdvantureState : GameState
    {
        private EntityManager _entityManager; 
        public AdvantureState(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            _entityManager.OnEnter();
        }
       

        public override void OnUpdate()
        {
            _entityManager.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            _entityManager.OnExit();
        }
    }

}
