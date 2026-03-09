namespace Terramorphers
{
    public abstract class GameState
    {
        protected GameState(){}
        public virtual void OnEnter(){}
        public virtual void OnExit(){}
        public virtual void OnUpdate(){}
        public virtual void OnFixedUpdate(){}
        public virtual void OnLateUpdate(){}
    }
}