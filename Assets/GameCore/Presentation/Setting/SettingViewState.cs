using R3;
using WEngine.MVP;

namespace GameCore.Presentation.Setting
{
    public class SettingViewState : ViewState
    {
        public ReactiveProperty<float> SoundVolume { get; } = new();
        public ReactiveProperty<float> MusicVolume { get; } = new();
        public ReactiveCommand SoundMutedCommand { get; } = new();
        public ReactiveCommand MusicMutedCommand { get; } = new();
        public ReactiveProperty<bool> SoundMuted { get; } = new();
        public ReactiveProperty<bool> MusicMuted { get; } = new();
        public ReactiveCommand ExitCommand { get; } = new();
     
        public ReactiveCommand CloseCommand { get; } = new();
    }

}
