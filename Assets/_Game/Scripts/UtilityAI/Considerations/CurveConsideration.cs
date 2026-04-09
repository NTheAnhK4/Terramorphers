using System;
using UnityEngine;

namespace UtilityAI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/CurveConsideration", fileName = "CurveConsideration")]
    public class CurveConsideration : Consideration
    {
        [SerializeField] private string key;
        [SerializeField] private AnimationCurve curve;
        public override float Evaluate(Context context)
        {
            float rawInput = context.GetData<float>(key);
            return Mathf.Clamp01(curve.Evaluate(rawInput));
        }

        private void OnValidate()
        {
            if (curve == null || curve.length == 0)
            {
                curve = new AnimationCurve(
                    new Keyframe(0, 0),
                    new Keyframe(1, 1));
            }
        }
    }
}