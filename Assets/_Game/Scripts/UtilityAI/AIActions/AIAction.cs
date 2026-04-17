
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using UnityEngine;

using UtilityAI.Considerations;


namespace UtilityAI.AIActions
{
    public abstract class AIAction 
    { 
        protected int considerationID;
        protected Enemy entity;

        public AIAction(Enemy entity, int considerationID)
        {
            this.entity = entity;
            this.considerationID = considerationID;
        }
        public void Initalize(Context context){}
        protected virtual void SetData(ConsiderationSystem system, ConsiderationContext context){}
        public virtual float CaculateUtility(ConsiderationSystem system, ConsiderationContext context)
        {
            SetData(system, context);
            return system.Evaluate(considerationID, context) * GetBestOption(system, context);
        }

        public abstract float GetBestOption(ConsiderationSystem system, ConsiderationContext context);

        public abstract UniTask Execute(Context context);
    }


}


