using System;
using DG.Tweening;
using GameCore.Usecase.Audio;
using GameCore.Utility.Audio.GameAudio;
using JSAM;
using R3;
namespace Terramorphers
{
    public class AdvantureState : GameState
    {
        private EntityManager _entityManager;
        private InputManager _inputManager;
        private GameManager _gameManager;
        private DisposableBag _bag;
        private AudioUseCase _audioUseCase;
        public AdvantureState(EntityManager entityManager, InputManager inputManager,
            GameManager gameManager, AudioUseCase audioUseCase)
        {
            _entityManager = entityManager;
            _inputManager = inputManager;
            _gameManager = gameManager;
            _audioUseCase = audioUseCase;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            PlayMusic();
            _gameManager.GamePlayPresenter.IsShowingUI.Subscribe(_inputManager.StopInput).AddTo(ref _bag);
            _entityManager.OnEnter();
        }

        private void PlayMusic()
        {
            try
            {
                if (AudioManager.MusicMuted) return;
                AudioManager.FadeMusicIn(EMusicType.AdvantureMusic, .15f);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
       

        public override void OnUpdate()
        {
            _entityManager.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
            if (AudioManager.TryGetPlayingMusic(EMusicType.AdvantureMusic, out MusicChannelHelper audio))
            {
                audio.AudioSource.DOFade(0, 0.5f)
                    .OnComplete(() =>
                    {
                        AudioManager.StopMusic(EMusicType.AdvantureMusic, stopInstantly: true);
                    });
            }
            _bag.Dispose();
            _entityManager.OnExit();
        }
    }

}
