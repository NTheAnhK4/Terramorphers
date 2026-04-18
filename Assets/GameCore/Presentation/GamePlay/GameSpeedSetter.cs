using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Presentation.GamePlay
{
    public class GameSpeedSetter : MonoBehaviour
    {
        [SerializeField] private Button btn;
        [SerializeField] private TextMeshProUGUI speedText;

        [SerializeField] private List<float> speedAvailables = new List<float>()
        {
            1, 1.5f, 2f
        };

        private int currentSpeedID = -1;

        private void Start()
        {
            ResetSpeed();
            btn.onClick.AddListener(SetSpeed);
        }

        

        private void OnDestroy()
        {
            btn.onClick.RemoveAllListeners();
        }

        private void SetSpeed()
        {
            currentSpeedID = (currentSpeedID + 1) % speedAvailables.Count;
            float value = speedAvailables[currentSpeedID];
            speedText.text = $"{value}X";
            Time.timeScale = value;

        }

        private void ResetSpeed()
        {
            currentSpeedID = -1;
            SetSpeed();
        }
    }
}