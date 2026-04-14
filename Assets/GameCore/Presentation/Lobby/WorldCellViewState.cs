using GameCore.Domain.Level;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.Lobby
{
    public class WorldCellViewState : ViewState
    {
        public ReactiveProperty<LevelMetadata> LevelMetadata { get; } = new ReactiveProperty<LevelMetadata>();
        public ReactiveCommand EnterWorldCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<bool> IsLevelUnlock { get; } = new ReactiveProperty<bool>();
    }
}