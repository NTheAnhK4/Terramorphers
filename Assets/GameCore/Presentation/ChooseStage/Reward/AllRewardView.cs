using System.Collections;
using System.Collections.Generic;
using CoreGame;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Domain.Level;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage.Reward
{
    public class AllRewardView : AppView<AllRewardViewState>
    {
        
        
        [SerializeField, TabGroup("Components")] private RewardView rewardViewPrefab;
        [SerializeField, TabGroup("Components")] private Transform holder;
        [SerializeField,TabGroup("Animation")] private AnimationCurve showAnimCurve;

        [SerializeField, TabGroup("Animation")]
        private RectTransform _rectTransform;

        private AllRewardViewState _state;
        protected override UniTask Initialize(AllRewardViewState state)
        {
            _state = state;
            return UniTask.CompletedTask;
        }

        public void SpawnItem(IReadOnlyList<StageRewardData> datas, IObjectResolver resolver)
        {
            foreach (var stageRewardData in datas)
            {
                var reward = PoolingManager.Spawn(rewardViewPrefab.gameObject, holder).GetComponent<RewardView>();
                resolver.Inject(reward);
                reward.Init(stageRewardData);
            }
            
        }

        public async UniTask ShowItem()
        {
            DOTween.Kill(transform);
            _rectTransform.localScale = new Vector3(1, 0, 1);
            var seq = DOTween.Sequence().SetTarget(transform);
            seq.AppendInterval(.15f)
                .Append(_rectTransform.DOScaleY(1, .5f).SetEase(showAnimCurve));
            await seq.AwaitForComplete();
            _state.FinishShow.Execute(default);
        }

        public void HideItem()
        {
            DOTween.Kill(transform);
            _rectTransform.DOScaleY(0, 0.3f)
                .SetEase(Ease.InQuad).SetTarget(transform);
        }

        public void WillExit()
        {
            foreach (var go in holder.Children())
            {
                PoolingManager.Despawn(go.gameObject);
            }
        }
    }
}

