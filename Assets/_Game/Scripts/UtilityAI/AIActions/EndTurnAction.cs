using Cysharp.Threading.Tasks;
using GameCore.Commands;
using UnityEngine;

namespace UtilityAI.AIActions
{
    [CreateAssetMenu(menuName = "UtilityAI/AIActions/EndTurnAction", fileName = "EndTurnAction")]
    public class EndTurnAction : AIAction
    {
        public override async UniTask Execute(Context context)
        {
            await UniTask.Delay(1000, cancellationToken: context.Entity.GetCancellationTokenOnDestroy());
            _ =  context.Entity.Publisher.PublishAsync(new EndEntityTurnCommand());
            
        }
    }
}