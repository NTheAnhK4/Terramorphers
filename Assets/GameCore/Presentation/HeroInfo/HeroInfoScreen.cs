using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Presentation.HeroInfo.CurrentSkill;
using GameCore.Presentation.HeroInfo.SkillFrame;
using GameCore.Presentation.Skill;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.HeroInfo
{
    public class HeroInfoScreen : Screen<HeroInfoViewState>
    {
        [SerializeField] private Button chooseSkillBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private SkillFrameView skillFrameViewPrefab;
        [SerializeField] private Transform skillFrameHolder;
        [SerializeField] private SkillInfoView skillInfoView;
        [SerializeField] private CurrentSkillView currentSkillView;
        public SkillInfoView SkillInfoView => skillInfoView;

        public CurrentSkillView CurrentSkillView => currentSkillView;

        public override UniTask InitializeState(HeroInfoViewState state, Memory<object> args)
        {
            skillInfoView.gameObject.SetActive(false);
            exitBtn.SubscribeToCommand(state.ExitCommand).AddTo(this);
            chooseSkillBtn.SubscribeToCommand(state.HandleSelectionCommand).AddTo(this);
            state.SelectSkillCommand.Subscribe(SelectSkill).AddTo(this);
            state.UnselectSkillCommand.Subscribe(UnselectSkill).AddTo(this);
            state.PreviewSkillCommand.Subscribe(SelectSkill).AddTo(this);
            return UniTask.CompletedTask;
        }

        public SkillFrameView AddSkillFrameView() => Instantiate(skillFrameViewPrefab, skillFrameHolder);

        private void SelectSkill(int _)
        {
            DOTween.Kill(chooseSkillBtn.transform);
            var sequence = DOTween.Sequence();
            sequence.Append(chooseSkillBtn.transform.DOScale(1.1f, .2f))
                .Append(chooseSkillBtn.transform.DOScale(1, .2f)).SetTarget(chooseSkillBtn.transform);
            
        }

        private void UnselectSkill(int _)
        {
            DOTween.Kill(chooseSkillBtn.transform);
            var sequence = DOTween.Sequence();
            sequence.Append(chooseSkillBtn.transform.DOScale(.9f, .2f))
                .Append(chooseSkillBtn.transform.DOScale(1, .2f)).SetTarget(chooseSkillBtn.transform);
        }
        
    }
}