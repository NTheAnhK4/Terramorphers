using R3;
using WEngine.MVP;

namespace GameCore.Presentation.Loading
{
    public class LoadingViewState : ViewState
    {
        public ReactiveProperty<float> Progress { get; } = new ReactiveProperty<float>();
        public ReactiveCommand CloseActivity { get; } = new();
     
    }
}