using System.Collections.Generic;
using GameCore.Domain.Skill;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.GamePlay
{
    public class GamePlayViewState : ViewState
    {
        public ReactiveCommand EndTurnCommand { get; } = new();
        public ReactiveProperty<bool> IsActiveEndTurnCommand { get; } = new();
        public ReactiveProperty<int> CurrentRound { get; } = new();
        public List<SkillMetadata> SkillMetadatas = new();
        public ReactiveProperty<(int mana, int maxMana)> Mana { get; } = new();
        public ReactiveProperty<(int stamina, int maxStamina)> Stamina { get; } = new();
      
        public ReactiveCommand ExitCommand { get; } = new();
  
        public ReactiveCommand ObjectiveCommand { get; } = new();
        public ReactiveProperty<bool> ShowSkillInfoCommand { get; } = new();

    }
}