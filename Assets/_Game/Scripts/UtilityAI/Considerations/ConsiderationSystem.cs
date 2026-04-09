
using Sirenix.OdinInspector;
using UnityEngine;

namespace UtilityAI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/ConsiderationSystem", fileName = "ConsiderationSystem")]
    public class ConsiderationSystem : SerializedScriptableObject
    {
        public enum ConsiderationType
        {
            Constant,
            Curve,
            Inverse,
            Composite
        }
        
    }

}
