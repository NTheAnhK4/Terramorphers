using System;
using UnityEngine;
using UtilityAI.ActionDataBuilder;

namespace UtilityAI.Consideration
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/InverseConsideration", fileName = "InverseConsideration")]
    public class InverseConsideration : Consideration
    {
        [Serializable]
        public class Data
        {
            [SerializeReference] public Consideration Consideration;
        }

        [SerializeField] private Data data;
        public override float Evaluate(Context context, ActionExecutionData executionData)
        {
            return 1 - data.Consideration.Evaluate(context, executionData);
        }
    }
}

