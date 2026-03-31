
using GameCore.Utility.Shape;
using UnityEngine;

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

