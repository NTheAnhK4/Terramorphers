using CoreGame;

namespace Terramorphers
{
    public abstract class TerramorphersEntity : Entity
    {
        protected ITile currentTile;
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
        public abstract void SetTile(ITile tile);
        public abstract bool IsDead();
        public ITile CurrentTile => currentTile;
    }
}