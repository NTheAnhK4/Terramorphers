using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Audio;
using GameCore.Utility.Audio.GameAudio;
using JSAM;
using UnityEngine;

namespace Terramorphers
{
    public class LobbyState : GameState
    {
        private readonly TransitionService _transitionService;
        private readonly GameManager _gameManager;
        private CancellationTokenSource _cancellationTokenSource;
        private AudioUseCase _audioUseCase;

        public LobbyState(GameManager gameManager, TransitionService transitionService, AudioUseCase audioUseCase)
        {
            _gameManager = gameManager;
            _transitionService = transitionService;
            _audioUseCase = audioUseCase;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            var oldCts = _cancellationTokenSource;
            _cancellationTokenSource = new CancellationTokenSource();

            oldCts?.Cancel();
            oldCts?.Dispose();
            ShowLobbyScreen().Forget();
            PlayMusic().Forget();
        }
          private async UniTask PlayMusic()
          {
              try
              {
                  _audioUseCase.SetUp();
                  await UniTask.WaitUntil(
                      () => AudioManager.Instance != null && AudioManager.Instance.Initialized, 
                      cancellationToken:_cancellationTokenSource.Token);
                  if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
                      return;

                  if(AudioManager.MusicMuted) return;
                  AudioManager.FadeMusicIn(EMusicType.LobbyMusic, .15f);
                 
              }
              catch(OperationCanceledException){}
              
        }
        private async  UniTask ShowLobbyScreen()
        {
            var lobbyPresenter = await _transitionService.ShowLobbyScreen();
        }

        public override void OnExit()
        {
            base.OnExit();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;    
            if (AudioManager.TryGetPlayingMusic(EMusicType.LobbyMusic, out MusicChannelHelper audio))
            {
                audio.AudioSource.DOFade(0, 0.5f)
                    .OnComplete(() =>
                    {
                        AudioManager.StopMusic(EMusicType.LobbyMusic, stopInstantly: true);
                    });
            }
        }
            
    }

}
