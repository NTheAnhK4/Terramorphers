using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terramorphers
{
    public interface ITile
    {
        ETileState CurrentState { get; protected set; }
        void ChangeState(ETileState newState);
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

