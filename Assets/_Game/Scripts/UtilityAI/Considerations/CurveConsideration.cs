using System;
using UnityEngine;
using UtilityAI.ActionDataBuilder;


namespace UtilityAI.Consideration
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/CurveConsideration", fileName = "CurveConsideration")]
    public class CurveConsideration : Consideration
    {
        [Serializable]
        public class Data
        {
            public AnimationCurve Curve;
            public string ContextKey;
        }

        [SerializeField] private Data data;
       

        public override float Evaluate(Context context, ActionExecutionData executionData)
        {
            float inputValue = context.GetData<float>(data.ContextKey);
            float utility = data.Curve.Evaluate(inputValue);
            return Mathf.Clamp01(utility);
        }

        private void OnValidate()
        {
            data = new Data();
            data.Curve = new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(1f, 0f)
            );
        }

       
    }

}
