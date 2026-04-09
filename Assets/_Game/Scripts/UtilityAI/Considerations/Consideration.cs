using UnityEngine;

namespace UtilityAI.Considerations
{
    public abstract class Consideration : ScriptableObject
    {
        public abstract float Evaluate(Context context);
    }
}