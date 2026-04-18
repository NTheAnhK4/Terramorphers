using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Presentation.Shared;
using GameCore.Utility.Audio.GameAudio;
using JSAM;
using VContainer;

namespace Terramorphers
{
    public class LoseState : GameState
    {
        private TransitionService _transitionService;

        [Inject]
        public void Constructor(TransitionService transitionService)
        {
            _transitionService = transitionService;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            PlayMusic();
            EnterAsync().Forget();
        }

        private async UniTask EnterAsync()
        {
            var presentor = await _transitionService.ShowLoseGameModal();
        }
        private void PlayMusic()
        {
            var audio = AudioManager.PlayMusic(EMusicType.DefeatMusic);
            if (!AudioManager.MusicMuted)
            {
                audio.AudioSource.volume = 0;
                audio.AudioSource.DOFade(1, .15f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (AudioManager.TryGetPlayingMusic(EMusicType.DefeatMusic, out MusicChannelHelper audio))
            {
                audio.AudioSource.DOFade(0, 0.5f)
                    .OnComplete(() =>
                    {
                        AudioManager.StopMusic(EMusicType.DefeatMusic, stopInstantly: true);
                    });
            }
        }
    }
}