using UnityEngine;

namespace UtilityAI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/InverseConsideration", fileName = "InverseConsideration")]
    public class InverseConsideration : Consideration
    {
        [SerializeField] private Consideration consideration;
        public override float Evaluate(Context context)
        {
            return 1 - consideration.Evaluate(context);
        }
    }
}