using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.StageObjective
{
    public class StageObjectiveModal : Modal<StageObjectiveViewState>
    {
        [SerializeField] private Button exitBtn;
        [SerializeField] private Transform modalHolder;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private List<ObjectiveItem> objectives = new();
        
        public override async UniTask InitializeState(StageObjectiveViewState state, Memory<object> args)
        {
            exitBtn.SubscribeToCommand(state.CloseCommand);
            for (int i = 0; i < objectives.Count; ++i)
            {
                var objective = objectives[i];
                state.ObjectiveDescriptions[i].Subscribe(value => objective.SetDescription(value)).AddTo(this);
                state.IsObjectiveFailed[i].Subscribe(value => objective.SetFailed(value)).AddTo(this);
            }
            //await ShowModal();
        }
        //
        // private async UniTask ShowModal()
        // {
        //     transform.localScale = Vector3.one;
        //     canvasGroup.alpha = 1;
        //     modalHolder.localScale = Vector3.zero;
        //     DOTween.Kill(transform);
        //     var tween = modalHolder.DOScale(1, .5f).SetEase(Ease.OutQuad).SetTarget(transform);
        //     await tween.AwaitForComplete();
        // }

        // public async UniTask CloseModal()
        // {
        //     DOTween.Kill(transform);
        //     Sequence sequence = DOTween.Sequence();
        //     sequence.Append(transform.DOScale(5, .2f)).SetEase(Ease.OutQuad).SetTarget(transform)
        //         .Join(canvasGroup.DOFade(0, .2f).SetEase(Ease.OutQuad));
        //     await sequence.AwaitForComplete();
        // }
    }
}