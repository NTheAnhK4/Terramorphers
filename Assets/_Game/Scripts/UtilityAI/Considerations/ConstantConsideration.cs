using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UtilityAI.ActionDataBuilder;

namespace UtilityAI.Consideration
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/ConstantConsideration", fileName = "ConstantConsideration")]
    public class ConstantConsideration : Consideration
    {
        [Serializable]
        public class Data
        {
            public float Value;
        }

       
     
        [SerializeField] private Data data;

        

        public override float Evaluate(Context context, ActionExecutionData executionData) => data.Value;
    }
}