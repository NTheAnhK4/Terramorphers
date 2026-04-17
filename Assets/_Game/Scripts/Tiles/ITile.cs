
using GameCore.Domain.Tile;
using GameCore.Utility;
using GameCore.Utility.Shape;
using UnityEngine;
using UtilityAI;

namespace Terramorphers
{
    public interface ITile
    {
        ETileState CurrentState { get; protected set; }
        void ChangeState(ETileState newState, int cost = 0);
        bool IsPassable();
        bool IsBlockVisibility();
        Transform Transform { get; }
        int GetMoveCost();
        Cube Index { get; set; }
        TerramorphersEntity CurrentOccupant { get; set; }
        TileMetadata TileMetadata { get; set; }
        Context Context { get; protected set; }

        void Init(TileMetadata tileMetadata);

    }

    public enum ETileState
    {
        Normal,
        Movable,
        SkillApplicable,
        SelfTargetSkill,
        AllyTargetSkill,
        EnemyTargetSkill,
        TileTargetSkill,
    }
}

