using CoreGame;
using Sirenix.OdinInspector;
using Terramorphers.Stats;
using UnityEngine;

namespace Terramorphers
{
    public abstract class TerramorphersEntity : Entity
    {
        [SerializeField, TabGroup("Data")] public float MoveSpeed = 1;
        [SerializeField, TabGroup("Components")]
        protected StatsSystem statsSystem;
        protected ITile currentTile;
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
        public abstract void SetTile(ITile tile);
        public abstract bool IsDead();
        public ITile CurrentTile => currentTile;
        protected int _teamID;

        public int TeamID => _teamID;

        public StatsSystem StatsSystem => statsSystem;

        public virtual void Init(EntityMetadata metadata, int teamID)
        {
            statsSystem = new StatsSystem(metadata.EntityStats);
            _teamID = teamID;
        }
    }
}