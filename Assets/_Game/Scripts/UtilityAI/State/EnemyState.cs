
using System;
using CoreGame;
using Cysharp.Threading.Tasks;
using GameCore.Utility;



namespace UtilityAI.State
{
    public abstract class EnemyState<Tdata> : State<Enemy> where Tdata : StateData
    {
        protected Tdata data;

      
        public int animHash;

       
      

       
      
        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            if (stateData == null || stateData is not Tdata tdata) return;
            data = tdata;
           
        }

        public virtual UniTask Execute(Context context)
        {
            return UniTask.CompletedTask;
        }

       


        protected EnemyState(Enemy entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        protected EnemyState(Enemy entity, int animationHash) : base(entity, animationHash)
        {
        }

        protected EnemyState(Enemy entity) : base(entity)
        {
        }

        protected EnemyState(Enemy entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }
    }
}