using R3;
using WEngine.MVP;

namespace GameCore.Presentation.EntityInfo
{
    public class EntityInfoViewState : ViewState
    {
        public ReactiveCommand OnClose { get; } = new();
    }
}