using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Presentation.StageObjective
{
    public class ObjectiveItem : MonoBehaviour
    {
        [SerializeField] private Sprite starOn;
        [SerializeField] private Sprite starOff;
        [SerializeField] private Image starImg;
        [SerializeField] private TextMeshProUGUI description;
        public void SetDescription(string value) => description.text = value;

        public void SetFailed(bool isFailed)
        {
            starImg.sprite = isFailed ? starOff : starOn;
        }
    }
}