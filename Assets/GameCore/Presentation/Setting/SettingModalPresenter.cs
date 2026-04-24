using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Audio;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Audio;
using R3;
using UnityEngine;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.Setting
{
    public class SettingModalPresenter : ModalPresenter<SettingModal, SettingViewState>
    {
        private AudioUseCase _audioUseCase;
        private SettingViewState _state;
        private AudioModel _audioModel;
        private TransitionService _transitionService;
        [Inject]
        public void Constructor(AudioUseCase audioUseCase, TransitionService transitionService)
        {
            _audioUseCase = audioUseCase;
            _transitionService = transitionService;
        }
        public SettingModalPresenter(SettingModal view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, SettingViewState state, SettingModal view)
        {
            _state = state;
            _audioModel = _audioUseCase.GetModel();
            state.MusicMuted.Value = _audioModel.MusicMuted;
            state.SoundMuted.Value = _audioModel.SoundMuted;

            state.MusicVolume.Value = _audioModel.MusicVolume;
            state.SoundVolume.Value = _audioModel.SoundVolume;
            state.MusicMutedCommand.Subscribe(ToggleMusic).AddTo(view);
            state.SoundMutedCommand.Subscribe(ToggleSound).AddTo(view);
            state.MusicVolume.Subscribe(SetMusicVolume).AddTo(view);
            state.SoundVolume.Subscribe(SetSoundVolume).AddTo(view);
            
            state.CloseCommand.Subscribe(OnClose).AddTo(view);
            state.ExitCommand.Subscribe(OnExit).AddTo(view);
            return UniTask.CompletedTask;
        }

        private void ToggleMusic(Unit _)
        {
            _state.MusicMuted.Value = !_state.MusicMuted.Value;
            _audioUseCase.ToggleMusicMuted(_audioModel, _state.MusicMuted.Value);
        }


        private void ToggleSound(Unit _)
        {
            _state.SoundMuted.Value = !_state.SoundMuted.Value;
            _audioUseCase.ToggleSoundMuted(_audioModel, _state.SoundMuted.Value);
        }

        private void SetMusicVolume(float value) =>  _audioUseCase.SetMusicVolume(_audioModel,value);
        private void SetSoundVolume(float value) => _audioUseCase.SetSoundVolume(_audioModel, value);
        private void OnClose(Unit _) => _transitionService.ClosePopup().Forget();

        private void OnExit(Unit _)
        {
            try
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
            }
            catch(Exception){}
        }
    }
}