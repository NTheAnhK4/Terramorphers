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
        private ReactiveCommand<int> _previewCommand;
        private int currentSelectSkill;
        private ReactiveCommand _handleSelectionCommand;
        private CurrentSkillViewState _state;
        private ISkillDatabase _skillDatabase;
        private SkillModel _skillModel;
        private ReactiveCommand<int> _selectCommand;
        private ReactiveCommand<int> _unselectCommand;
       

        [Inject]
        public void Constructor(ISkillRepository skillRepository, SkillUseCase skillUseCase)
        {
            _skillRepository = skillRepository;
            _skillUseCase = skillUseCase;
          
        }
        public CurrentSkillPresenter(CurrentSkillView view,ReactiveCommand<int> previewCommand,
            ReactiveCommand handleSelectionCommand, ReactiveCommand<int> selectCommand, ReactiveCommand<int> unselectCommand) : base(view)
        {
            _previewCommand = previewCommand;
            _handleSelectionCommand = handleSelectionCommand;
            _selectCommand = selectCommand;
            _unselectCommand = unselectCommand;
        }

        protected override UniTask Initialize(CurrentSkillViewState state, CurrentSkillView view)
        {
            _state = state;
            _skillDatabase = _skillRepository.Get();
            _skillModel = _skillUseCase.GetModel();
            state.SkillMetadatas.Clear();
            _previewCommand.Subscribe(OnPreviewSkill).AddTo(view);
            _handleSelectionCommand.Subscribe(OnChooseSkill).AddTo(view);
            for(int i = 0; i < 7; ++i) state.SkillMetadatas.Add(new ReactiveProperty<SkillMetadata>());
            state.SelectSkill.SubscribeToCommand(_previewCommand);
            for (int i = 0; i < _skillModel.CurrentSkills.Count; ++i)
            {
                var skillMetadata = _skillDatabase.GetByType(_skillModel.CurrentSkills[i]);
                _selectCommand.Execute(_skillModel.CurrentSkills[i]);
                state.SkillMetadatas[i].Value = skillMetadata;
            }
            
            
            return UniTask.CompletedTask;
        }

        private void OnPreviewSkill(int id) => currentSelectSkill = id;

        private void OnChooseSkill(Unit _)
        {
            if (!_skillUseCase.IsSkillUnlock(currentSelectSkill)) return;
            foreach (var skillMetadata in _state.SkillMetadatas)
            {
                if(skillMetadata.Value == null) continue;
                if (currentSelectSkill == skillMetadata.Value.SkillID)
                {
                    _skillUseCase.RemoveSkill(_skillModel,currentSelectSkill);
                   
                    skillMetadata.Value = null;
                    _unselectCommand.Execute(currentSelectSkill);
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
                    _selectCommand.Execute(currentSelectSkill);
                    return;
                }
            }
            //notifi full
        }
    }
}