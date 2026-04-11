
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using UnityEngine;

using UtilityAI.Considerations;


namespace UtilityAI.AIActions
{
    public abstract class AIAction 
    { 
        protected int considerationID;
      

        public AIAction(int considerationID)
        {
            this.considerationID = considerationID;
        }
        public void Initalize(Context context){}

        public float CaculateUtility(ConsiderationSystem system, ConsiderationContext context) => system.Evaluate(considerationID, context) * GetBestOption(system, context);
        public abstract float GetBestOption(ConsiderationSystem system, ConsiderationContext context);

        public abstract UniTask Execute(Context context);
    }


}


