
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

    }

    public enum ETileState
    {
        Normal,
        Movable,
        SkillApplicable,
    }
}

