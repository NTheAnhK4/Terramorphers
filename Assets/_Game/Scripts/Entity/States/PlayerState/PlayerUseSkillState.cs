
using CoreGame;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Skill;

using Terramorphers.Command;
using UnityEngine;

namespace Terramorphers.States.PlayerState
{
    public class PlayerUseSkillData : StateData
    {
        public ITile SelectedTile { get; set; }
        public int SkillID { get; set; }
    }
    public class PlayerUseSkillState : State<Player>
    {
        private PlayerUseSkillData data;
        private SkillMetadata skillMetadata;
       

        public PlayerUseSkillState(Player entity, int animationHash) : base(entity, animationHash)
        {
        }

   

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            if (stateData == null || stateData is not PlayerUseSkillData useSkillData)
            {
                Debug.Log($"[Test] player use skill data is null");
                entity.ChangeState(entity.PlayerSelectMoveTileState);
                return;
            }

            data = useSkillData;
            entity.Publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = false });
            entity.Publisher.PublishAsync(new EnableSkillCommand() { IsEnable = false });
            entity.Publisher.PublishAsync(new ClearSpecialTilesCommand());
            skillMetadata =  entity.SkillManager.GetSkillMetadata(data.SkillID);
            if (skillMetadata == null)
            {
                Debug.Log($"[Test] cannot get skillMetadata for {data.SkillID}");
                entity.ChangeState(entity.PlayerSelectMoveTileState);
                return;
            }
            if (entity.RemainMana < skillMetadata.SkillCosts)
            {
                Debug.Log($"[Test] mana is not enough");
                entity.ChangeState(entity.PlayerSelectMoveTileState);
                return;
            }

            Vector3 direction = data.SelectedTile.Transform.position - entity.transform.position;
            entity.SetDirection(direction);
            
            entity.RemainMana -= skillMetadata.SkillCosts;
        }
        
        public override void AnimationTrigger()
        { 
            skillMetadata.Apply<ITile>(data.SelectedTile, entity.GetCancellationTokenOnDestroy()).Forget();
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            entity.ChangeState(entity.PlayerSelectMoveTileState);
        }
    }
}