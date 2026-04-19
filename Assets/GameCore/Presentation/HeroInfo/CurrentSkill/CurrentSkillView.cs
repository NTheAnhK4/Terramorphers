using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using R3;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo.CurrentSkill
{
    public class CurrentSkillView : AppView<CurrentSkillViewState>
    {
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private List<Image> skillImages = new();
        [SerializeField] private List<Button> skillBtns = new();
        private CurrentSkillViewState _state;
        protected override UniTask Initialize(CurrentSkillViewState state)
        {
            _state = state;
            for (int i = 0; i < skillImages.Count; ++i)
            {
                int index = i;
             
                _state.SkillMetadatas[i].Subscribe(t => SetSkillData(index, t)).AddTo(this);
                
            }
            for(int i = 0; i < _state.SkillMetadatas.Count; ++i) SetSkillData(i, _state.SkillMetadatas[i].Value);
            return UniTask.CompletedTask;
        }

        private void SetSkillData(int index,SkillMetadata skillMetadata)
        {
            Button skillBtn = skillBtns[index];
             skillBtn.onClick.RemoveAllListeners();
            if (skillMetadata != null)
            {
                skillImages[index].sprite = skillMetadata.SkillSprite;
               
               
                skillBtn.onClick.AddListener(() => _state.SelectSkill.Execute(skillMetadata.SkillID));
            }
            else  skillImages[index].sprite = emptySprite;
          
        }

       
    }

}
