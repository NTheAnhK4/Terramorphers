
using GameCore.Utility.Vibration;
using JSAM;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Utility.Audio
{
    [RequireComponent(typeof(Button))]
    public class SoundPlayOnButton : BaseAudioFeedback<SoundFileObject>
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            VibrationUti.Vibrate(100);
            AudioManager.PlaySound(audio);
        }
    }
}