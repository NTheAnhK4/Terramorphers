
using Cysharp.Threading.Tasks;

using UnityEngine;

using UtilityAI.Considerations;


namespace UtilityAI.AIActions
{
    public abstract class AIAction : ScriptableObject
    {
        [SerializeField] protected Considerations.Consideration consideration;
        public void Initalize(Context context){}
        

        public float CaculateUtility(Context context) => consideration.Evaluate(context);

        public abstract UniTask Execute(Context context);
    }


}


