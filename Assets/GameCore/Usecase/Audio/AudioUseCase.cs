using GameCore.APIGateway.Audio;
using GameCore.Domain.Audio;
using JSAM;

namespace GameCore.Usecase.Audio
{
    public class AudioUseCase : BaseUseCase<AudioAPIGateway, AudioModel>
    {
        public AudioUseCase(AudioAPIGateway apiGateway)
        {
            _apiGateway = apiGateway;
        }
        public void SetUp()
        {
            var audioModel = GetModel();
            AudioManager.MusicMuted = audioModel.MusicMuted;
            AudioManager.SoundMuted = audioModel.SoundMuted;
            AudioManager.MusicVolume = audioModel.MusicVolume;
            AudioManager.SoundVolume = audioModel.SoundVolume;
        }

        public void SetSoundVolume(AudioModel model,float value)
        {
            AudioManager.SoundVolume = value;
            _apiGateway.SetSoundVolume(model,value);
        }

        public void SetMusicVolume(AudioModel model, float value)
        {
            AudioManager.MusicVolume = value;
            _apiGateway.SetMusicVolume(model,value);
        }

        public void ToggleSoundMuted(AudioModel model, bool isOn)
        {
            AudioManager.SoundMuted = isOn;
            _apiGateway.ToggleSoundMuted(model,isOn);
        }

        public void ToggleMusicMuted(AudioModel model, bool isOn)
        {
            AudioManager.MusicMuted = isOn;
            _apiGateway.ToggleMusicMuted(model, isOn);
        }
    }
}