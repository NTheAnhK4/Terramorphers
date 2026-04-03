using System;
using CoreGame;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
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
        public PlayerUseSkillState(Player entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public PlayerUseSkillState(Player entity, int animationHash) : base(entity, animationHash)
        {
        }

        public PlayerUseSkillState(Player entity) : base(entity)
        {
        }

        public PlayerUseSkillState(Player entity, Func<string> animNameFunc) : base(entity, animNameFunc)
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
            OnEnterAsync().Forget();
        }

        private async UniTask OnEnterAsync()
        {
            try
            {
                var skillMetadata = entity.SkillManager.GetSkillMetadata(data.SkillID);
                if (skillMetadata == null)
                {
                    Debug.Log($"[Test] cannot get skillMetadata for {data.SkillID}");
                    entity.ChangeState(entity.PlayerSelectMoveTileState);
                    return;
                }

                await skillMetadata.Apply<ITile>(data.SelectedTile, entity.GetCancellationTokenOnDestroy());
                entity.ChangeState(entity.PlayerSelectMoveTileState);
            }
            catch(OperationCanceledException){}
          
        }
    }
}