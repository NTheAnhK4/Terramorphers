using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Utility;
using UnityEngine;
using WEngine.MVP;
using UnityEngine.UI;
using R3;
namespace GameCore.Presentation.WinGame
{
    
    
    public class WinGameModal : Modal<WinGameViewState>
    {
        [SerializeField] private Button menuBtn;
        [SerializeField] private Button nextBtn;
        [SerializeField] private float showStarDuration = .5f;
        [SerializeField] private List<Image> starOffs = new();
        [SerializeField] private List<Image> starOns = new();
        
        public override UniTask InitializeState(WinGameViewState state, Memory<object> args)
        {
            menuBtn.SubscribeToCommand(state.ToMenuCommand).AddTo(this);
            nextBtn.SubscribeToCommand(state.NextLevelCommand).AddTo(this);
            state.TotalStars.Subscribe(value =>SetStars(value).Forget()).AddTo(this);
            SetStars(state.TotalStars.Value).Forget();
            return UniTask.CompletedTask;
        }

        private async UniTask SetStars(int totalStars)
        {
            try
            {
                DOTween.Kill(transform);
                for (int i = 0; i < 3; ++i)
                {
                    var starOff = starOffs[i];
                    var starOn = starOns[i];
                    var color = starOff.color;
                    color.a = 0f;
                    starOff.color = color;
                    color = starOn.color;
                    color.a = 0;
                    starOn.color = color;
                    starOff.transform.localScale = 2 * Vector3.one;
                    starOn.transform.localScale = 2 * Vector3.one;
                    starOn.gameObject.SetActive(false);

                }

                foreach (var starOff in starOffs)
                {

                    starOff.DOFade(1, showStarDuration).SetTarget(transform);
                    starOff.transform.DOScale(1, showStarDuration).SetTarget(transform);
                }

                starOns[0].gameObject.SetActive(true);
                starOns[0].DOFade(1, showStarDuration).SetTarget(transform);
                starOns[0].transform.DOScale(1, showStarDuration).SetTarget(transform);
                await UniTask.Delay(TimeSpan.FromSeconds(showStarDuration), cancellationToken:this.GetCancellationTokenOnDestroy());
                for (int i = 1; i < totalStars; ++i)
                {
                    var starOn = starOns[i];
                    starOn.gameObject.SetActive(true);
                    starOn.DOFade(1, showStarDuration).SetTarget(transform);
                    starOn.transform.DOScale(1, showStarDuration).SetTarget(transform);
                    await UniTask.Delay(TimeSpan.FromSeconds(showStarDuration), cancellationToken:this.GetCancellationTokenOnDestroy());
                }
            }
            catch (OperationCanceledException)
            {
            }

        }
    }

}
