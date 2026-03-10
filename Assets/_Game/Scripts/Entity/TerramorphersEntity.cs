using CoreGame;

namespace Terramorphers
{
    public abstract class TerramorphersEntity : Entity
    {
        protected abstract void OnEnter();
        protected abstract void OnUpdate();
        protected abstract void OnExit();
    }
}