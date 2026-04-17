
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using UnityEngine;
using WEngine.MVP;
using UnityEngine.UI;
namespace GameCore.Presentation.Loading
{
    public class LoadingView : Activity<LoadingViewState>
    {
        [SerializeField] private Image leftPanel, rightPanel;
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject fillHolder;
        private LoadingViewState _state;
        private const string TWEEN_ID = "LoadingViewAnimID";

        public async UniTask PreLoading()
        {
            ResetView();
            DOTween.Kill(TWEEN_ID);
           
            var openSeq = DOTween.Sequence().SetId(TWEEN_ID);
           
            openSeq
                .Append(leftPanel.transform.DOScaleX(1, 0.5f).SetEase(Ease.Linear))
                .Join(rightPanel.transform.DOScaleX(1, 0.5f).SetEase(Ease.Linear))
                .Join(leftPanel.DOFade(1, 0.2f))
                .Join(rightPanel.DOFade(1, 0.2f));

            await openSeq.AsyncWaitForCompletion();

            await UniTask.Delay(100, cancellationToken: this.GetCancellationTokenOnDestroy());

          
            fillHolder.SetActive(true);
        }
        public async UniTask Loading()
        {
            await fillImage.DOFillAmount(0.7f,.1f)
                .SetEase(Ease.OutQuad)
                .SetId(TWEEN_ID)
                .AsyncWaitForCompletion();
         
            await fillImage.DOFillAmount(0.9f, .5f)
                .SetEase(Ease.Linear)
                .SetId(TWEEN_ID)
                .AsyncWaitForCompletion();
            
        }


        private void FinishLoading(float value)
        {
           
            if (value < 1) return;

            CompleteAndHide().Forget();
        }
        private async UniTask CompleteAndHide()
        {
            DOTween.Kill(TWEEN_ID);

            
            await fillImage.DOFillAmount(1f, 0.2f)
                .SetEase(Ease.OutQuad)
                .SetId(TWEEN_ID)
                .AsyncWaitForCompletion();
            fillHolder.gameObject.SetActive(false);
            await Hide();
        }

        private async UniTask Hide()
        {
            DOTween.Kill(TWEEN_ID);

            var closeSeq = DOTween.Sequence().SetId(TWEEN_ID);

            closeSeq
                .Append(leftPanel.transform.DOScaleX(0, 0.25f).SetEase(Ease.InBack))
                .Join(rightPanel.transform.DOScaleX(0, 0.25f).SetEase(Ease.InBack))
                .AppendInterval(.15f)
                
                .Join(leftPanel.DOFade(0, 0.1f))
                .Join(rightPanel.DOFade(0, 0.1f));

            await closeSeq.AsyncWaitForCompletion();
            _state.CloseActivity.Execute(default);
        }

        public override UniTask InitializeState(LoadingViewState state, Memory<object> args)
        {
            _state = state;
            

            state.Progress
                .Subscribe(FinishLoading)
                .AddTo(this);

            return UniTask.CompletedTask;
        }
        private void ResetView()
        {
           
            DOTween.Kill(TWEEN_ID);

            fillImage.fillAmount = 0;
            fillHolder.SetActive(false);

            SetAlpha(leftPanel, 0.7f);
            SetAlpha(rightPanel, 0.7f);

            leftPanel.transform.localScale = new Vector3(0, 1, 1);
            rightPanel.transform.localScale = new Vector3(0, 1, 1);
        }
        private void SetAlpha(Image img, float a)
        {
            var c = img.color;
            c.a = a;
            img.color = c;
        }
    }
}

