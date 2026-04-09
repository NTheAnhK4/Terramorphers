using UnityEngine;

namespace UtilityAI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/ConstantConsideration", fileName = "ConstantConsideration")]
    public class ConstantConsideration : Consideration
    {
        [SerializeField] private float value;
        public override float Evaluate(Context context) => value;
    }
}