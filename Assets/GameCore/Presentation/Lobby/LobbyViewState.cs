
using R3;

using WEngine.MVP;

namespace GameCore.Presentation.Lobby
{


    public class LobbyViewState : ViewState
    {
        public ReactiveProperty<int> CurrentIndex { get; } = new ReactiveProperty<int>();
        public ReactiveCommand ShowHeroInfo { get; } = new ReactiveCommand();
    }
}
