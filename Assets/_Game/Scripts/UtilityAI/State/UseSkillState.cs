using System;
using System.Collections.Generic;
using CoreGame;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using Terramorphers;
using UnityEngine;


namespace UtilityAI.State
{
    public class UseSkillStateData : StateData
    {
        public int SkillID;
        public ITile TargetTile;
    }
    public class UseSkillState : EnemyState<UseSkillStateData>
    {
        private Dictionary<int,int> skillToAnim = new();
        private bool isFinishAnim = false;
        private Context _context;
      
        public UseSkillState(Enemy entity,IReadOnlyList<SkillConsiderationData> skillEvaluationDatas) : base(entity)
        {
            foreach (var skillEvaluationData in skillEvaluationDatas)
            {
                skillToAnim[skillEvaluationData.SkillID] = Animator.StringToHash(skillEvaluationData.AnimName);
            }
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            Vector3 direction = data.TargetTile.Transform.position - entity.transform.position;
            entity.SetDirection(direction);
        }


        public override async UniTask Execute(Context context)
        {
          
            _context = context;
            try
            {
                if (!skillToAnim.ContainsKey(data.SkillID)) return;
                if(entity.Anim.HasState(0, skillToAnim[data.SkillID])) entity.Anim.Play(skillToAnim[data.SkillID]);
                isFinishAnim = false;
                base.Execute(context);
                await UniTask.WaitUntil(() => isFinishAnim, cancellationToken: entity.GetCancellationTokenOnDestroy());
            }
            catch(OperationCanceledException){}
            
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
            var skillMetadata = entity.SkillManager.GetSkillMetadata(data.SkillID);
            if (skillMetadata == null) isFinishAnim = true;
            else
            {
                int currentMana = _context.GetData<int>(BlackBoardConstant.REMAIN_MANA_KEY);
                _context.SetData(BlackBoardConstant.REMAIN_MANA_KEY, currentMana - skillMetadata.SkillCosts);
               skillMetadata.Apply(data.TargetTile, entity.transform.GetCancellationTokenOnDestroy()).Forget();
            }
        }


        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
          
            isFinishAnim = true;
        }
    }
    
}