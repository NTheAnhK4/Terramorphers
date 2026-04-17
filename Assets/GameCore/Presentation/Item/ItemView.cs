
using UnityEngine;
using UnityEngine.UI;
namespace GameCore.Presentation.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image frameImage;
        [SerializeField] private Image enemyIcon;

        public void Init(Sprite enemySR)
        {
            enemyIcon.sprite = enemySR;
        }
    }
}

