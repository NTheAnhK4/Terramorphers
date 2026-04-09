using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UtilityAI.ActionDataBuilder;

namespace UtilityAI.Consideration
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/CompositeConsideration", fileName = "CompositeConsideration")]
    public class CompositeConsideration : Consideration
    {
        [Serializable]
        public class Data
        {
            public bool AllMustNoneZero = true;
            public EOperationType Operation = EOperationType.Max;
            public List<Consideration> Considerations = new();
        }
        public enum EOperationType{Average, Muttiply, Add, Subtract, Divide, Max, Min}

      
        [SerializeField] private Data data;
        public override float Evaluate(Context context, ActionExecutionData executionData)
        {
            if (data.Considerations.Count == 0) return 0;
            float result = data.Considerations[0].Evaluate(context, executionData);
            if (result == 0 && data.AllMustNoneZero) return 0;

            //Suggest 2 operation
            for (int i = 1; i < data.Considerations.Count; ++i)
            {
                float value = data.Considerations[i].Evaluate(context, executionData);
                if (value == 0 && data.AllMustNoneZero) return 0;
                switch (data.Operation)
                {
                    case EOperationType.Average:
                        result = (result + value) / 2;
                        break;
                    case EOperationType.Muttiply:
                        result *= value;
                        break;
                    case EOperationType.Add:
                        result += value;
                        break;
                    case EOperationType.Subtract:
                        result -= value;
                        break;
                    case EOperationType.Divide:
                        result /= value;
                        break;
                    case EOperationType.Max:
                        result = Mathf.Max(result, value);
                        break;
                    case EOperationType.Min:
                        result = Mathf.Min(result, value);
                        break;
                }
            }

            return Mathf.Clamp01(result); 
        }
    }
}

