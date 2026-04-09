

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Terramorphers;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityAI.ActionDataBuilder;


namespace UtilityAI.AIActions
{
    public abstract class AIAction : ScriptableObject
    {
        [FormerlySerializedAs("consideration")] public Consideration.Consideration Consideration;
        public List<ActionDataBuilder.ActionDataBuilder> DataBuilders;
        protected ActionExecutionData executionData;
        
        public void Initalize(Context context){}
        public void Build()
        {
            foreach (var builder in DataBuilders) builder.Build(executionData);
        }

        public float CaculateUtility(Context context) => Consideration.Evaluate(context, executionData);

        public abstract UniTask Execute(Context context);
    }


}


