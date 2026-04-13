using WEngine.MVP;

namespace GameCore.Presentation.Lobby
{
    public class LobbyPresenter : ScreenPresenter<LobbyScreen, LobbyViewState>
    {
        public LobbyPresenter(LobbyScreen view) : base(view)
        {
        }
    }
}