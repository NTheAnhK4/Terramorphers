using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;

namespace GameCore.Presentation.EntityInfo
{
    public class StatView : MonoBehaviour
    {
        [SerializeField, TabGroup("Components")]
        private Image iconImg;

        [SerializeField, TabGroup("Components")]
        private TextMeshProUGUI amountText;
        

      

        public void Setup(Sprite icon, string amount, Color amountColor)
        {
            iconImg.sprite = icon;
            amountText.text = amount;
            amountText.color = amountColor;
        }
    }

}
