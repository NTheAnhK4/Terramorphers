using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Domain.Level;

using TMPro;
using UnityEngine;
using WEngine.MVP;
using R3;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace GameCore.Presentation.ChooseStage
{
    public class ChooseStageModal : Modal<ChooseStageViewState>
    {
        [SerializeField, TabGroup("Components")] private Button closeBtn;
        [SerializeField, TabGroup("Components")] private Transform holder;
        [SerializeField, TabGroup("Components")] private StageView stagePrefab;
        [SerializeField, TabGroup("Components")] private TextMeshProUGUI levelText;

        [SerializeField, TabGroup("Animation")]
        private Image pannelImg;

        [SerializeField, TabGroup("Animation")]
        private RectTransform topRect;

        [SerializeField, TabGroup("Animation")]
        private RectTransform leftRect;

        [SerializeField, TabGroup("Animation")]
        private RectTransform rightRect;

        [SerializeField, TabGroup("Animation")]
        private AnimationCurve showRightCurve;

        private ChooseStageViewState _state;
      
     
        public override UniTask InitializeState(ChooseStageViewState state, Memory<object> args)
        {
            _state = state;
            state.LevelName.Subscribe(SetLevelName).AddTo(this);
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.AddListener(() => Hide().Forget());
           
            Show().Forget();
            return UniTask.CompletedTask;
        }

        private void SetLevelName(string value) => levelText.text = value;

        public StagePresenter CreateStage(int levelID,int stageID,LevelStageData levelStageData, ReactiveCommand onClose)
        {
            var stageView = Instantiate(stagePrefab, holder);
          
            StagePresenter stagePresenter = new StagePresenter(stageView,levelID,stageID, levelStageData, onClose);
            return stagePresenter;
        }

       public async UniTask Show()
       {
           try
           {
               DOTween.Kill(transform);
       
               var color = pannelImg.color;
               color.a = 0;
               pannelImg.color = color;

               topRect.anchoredPosition = new Vector2(0, 235);
               rightRect.transform.localScale = new Vector3(1, 0, 1);
               
       
               var seq = DOTween.Sequence().SetTarget(transform);
       
               seq.Append(pannelImg.DOFade(0.3f, 0.25f).SetEase(Ease.OutQuad));
       
               seq.Join(
                   topRect.DOAnchorPosY(0, .5f)
                         .SetEase(Ease.OutBack) 
               );
       
               seq.Insert(0.15f,
                   rightRect.DOScaleY(1,.5f).SetEase(showRightCurve)
               );
       
               await seq.ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());
           }
           catch (OperationCanceledException) { }
       }

       public async UniTask Hide()
       {
           try
           {
               DOTween.Kill(transform);

               var seq = DOTween.Sequence().SetTarget(transform);

               seq.Append(pannelImg.DOFade(0, 0.2f));

               seq.Join(
                   topRect.DOAnchorPosY(235, 0.3f)
                       .SetEase(Ease.InQuad)
               );

               seq.Join(
                   rightRect.DOScaleY(0, 0.3f)
                       .SetEase(Ease.InQuad)
               );

               await seq.ToUniTask(cancellationToken: this.GetCancellationTokenOnDestroy());

               closeBtn.onClick.RemoveAllListeners();
               _state.OnClose.Execute(default);
           }
           catch { }
       }
    }

}
