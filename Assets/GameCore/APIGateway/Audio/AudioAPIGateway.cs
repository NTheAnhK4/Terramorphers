using GameCore.Domain.Audio;

namespace GameCore.APIGateway.Audio
{
    public class AudioAPIGateway : BaseAPIGateway<AudioModel>
    {
        protected override string PlayerPrefsKey => "AudioData";
        protected override AudioModel CreateDefaultModel()
        {
            return new AudioModel()
            {
                SoundVolume = .2f,
                MusicVolume = .2f,
                SoundMuted = false,
                MusicMuted = false,
            };
        }

        public void SetSoundVolume(AudioModel model,float value)
        {
            model.SoundVolume = value;
            Update(model);
        }

        public void SetMusicVolume(AudioModel model, float value)
        {
            model.MusicVolume = value;
            Update(model);
        }

        public void ToggleMusicMuted(AudioModel model, bool isOn) => model.MusicMuted = isOn;
        public void ToggleSoundMuted(AudioModel model, bool isOn) => model.SoundMuted = isOn;
    }
}