
using Sirenix.OdinInspector;
using UnityEngine;
using UtilityAI.ActionDataBuilder;

namespace UtilityAI.Consideration{
    public abstract class Consideration : ScriptableObject
    {
        
        public abstract float Evaluate(Context context, ActionExecutionData executionData);
        
    }
 }
