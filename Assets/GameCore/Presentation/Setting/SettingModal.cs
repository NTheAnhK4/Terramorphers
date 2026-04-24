using System;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
using Sirenix.OdinInspector;

namespace GameCore.Presentation.Setting
{
    public class SettingModal : Modal<SettingViewState>
    {
        [SerializeField, TabGroup("Components")] private Slider soundSlider;
        [SerializeField, TabGroup("Components")] private Slider musicSlider;
        [SerializeField, TabGroup("Components")] private Button muteSoundBtn, muteMusicBtn;

        [SerializeField, TabGroup("Components")]
        private Button exitBtn, continueBtn, closeBtn;
        [SerializeField, TabGroup("Configs")] private Sprite musicOn, musicOff;
        [SerializeField, TabGroup("Configs")] private Sprite soundOn, soundOff;
        public override UniTask InitializeState(SettingViewState state, Memory<object> args)
        {
            muteMusicBtn.SubscribeToCommand(state.MusicMutedCommand).AddTo(this);
            muteSoundBtn.SubscribeToCommand(state.SoundMutedCommand).AddTo(this);
            soundSlider.value = state.SoundVolume.Value;
            musicSlider.value = state.MusicVolume.Value;

            state.MusicMuted.Subscribe(ToggleMusic).AddTo(this);
            state.SoundMuted.Subscribe(ToggleSound).AddTo(this);

            exitBtn.SubscribeToCommand(state.ExitCommand).AddTo(this);
            continueBtn.SubscribeToCommand(state.CloseCommand).AddTo(this);
            closeBtn.SubscribeToCommand(state.CloseCommand).AddTo(this);
            
            ToggleMusic(state.MusicMuted.Value);
            ToggleSound(state.SoundMuted.Value);
            
            soundSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.RemoveAllListeners();
            soundSlider.onValueChanged.AddListener(t => state.SoundVolume.Value = t); 
            musicSlider.onValueChanged.AddListener(t => state.MusicVolume.Value = t);
            return UniTask.CompletedTask;
        }

        private void ToggleSound(bool isOn)
        {
            muteSoundBtn.image.sprite = isOn ? soundOff : soundOn;
        }

        private void ToggleMusic(bool isOn)
        {
            muteMusicBtn.image.sprite = isOn ? musicOff : musicOn;
        }
    }
}