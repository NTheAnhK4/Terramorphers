using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Usecase.Skill;
using GameCore.Utility;
using R3;
using UnityEngine;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo.CurrentSkill
{
    public class CurrentSkillPresenter : AppViewPresenter<CurrentSkillView, CurrentSkillViewState>
    {
        private ISkillRepository _skillRepository;
        private SkillUseCase _skillUseCase;
        private ReactiveCommand<int> _showSkillInfo;
        private int currentSelectSkill;
        private ReactiveCommand _chooseSkillCommand;
        private CurrentSkillViewState _state;
        private ISkillDatabase _skillDatabase;
        private SkillModel _skillModel;

        [Inject]
        public void Constructor(ISkillRepository skillRepository, SkillUseCase skillUseCase)
        {
            _skillRepository = skillRepository;
            _skillUseCase = skillUseCase;
          
        }
        public CurrentSkillPresenter(CurrentSkillView view,ReactiveCommand<int> showSkillInfo, ReactiveCommand chooseSkillCommand) : base(view)
        {
            _showSkillInfo = showSkillInfo;
            _chooseSkillCommand = chooseSkillCommand;
        }

        protected override UniTask Initialize(CurrentSkillViewState state, CurrentSkillView view)
        {
            _state = state;
            _skillDatabase = _skillRepository.Get();
            _skillModel = _skillUseCase.GetModel();
            state.SkillMetadatas.Clear();
            _showSkillInfo.Subscribe(OnSelectSkill).AddTo(view);
            _chooseSkillCommand.Subscribe(OnChooseSkill).AddTo(view);
            for(int i = 0; i < 7; ++i) state.SkillMetadatas.Add(new ReactiveProperty<SkillMetadata>());
            state.SelectSkill.SubscribeToCommand(_showSkillInfo);
            for (int i = 0; i < _skillModel.CurrentSkills.Count; ++i)
            {
                var skillMetadata = _skillDatabase.GetByType(_skillModel.CurrentSkills[i]);
                state.SkillMetadatas[i].Value = skillMetadata;
            }
            
            
            return UniTask.CompletedTask;
        }

        private void OnSelectSkill(int id) => currentSelectSkill = id;

        private void OnChooseSkill(Unit _)
        {
            foreach (var skillMetadata in _state.SkillMetadatas)
            {
                if(skillMetadata.Value == null) continue;
                if (currentSelectSkill == skillMetadata.Value.SkillID)
                {
                    _skillUseCase.RemoveSkill(_skillModel,currentSelectSkill);
                   
                    skillMetadata.Value = null;
                    return;
                }
            }
            //add skill 
            foreach (var skillMetadata in _state.SkillMetadatas)
            {
                if (skillMetadata.Value == null)
                {
                    skillMetadata.Value = _skillDatabase.GetByType(currentSelectSkill);
                    _skillUseCase.AddSkill(_skillModel,currentSelectSkill);
                    
                    return;
                }
            }
            //notifi full
        }
    }
}