using CoreGame;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terramorphers
{
    public class EntityUI : ComponentBehaviour
    {
        [SerializeField] private TerramorphersEntity entity;

        [SerializeField, TabGroup("Health", Icon = SdfIconType.Heart, TextColor = "red")]
        private Image healthFill;

        [SerializeField, TabGroup("Health")] private TextMeshProUGUI healthText;
        private DisposableBag bag;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (entity == null) entity = GetComponentInParent<TerramorphersEntity>();
            if (healthFill == null) healthFill = transform.Find("Health/HealthBar/Mask/Fill").GetComponent<Image>();
            if (healthText == null) healthText = transform.Find("Health/Value").GetComponent<TextMeshProUGUI>();
        }

        protected override void Awake()
        {
            base.Awake();
            entity.OnInitialized += SubscribeEvent;
        }

       

        private void OnDestroy()
        {
            entity.OnInitialized -= SubscribeEvent;
            bag.Dispose();
        }

        private void SubscribeEvent()
        {
            entity.DataCache.RemainHP.Subscribe(OnHpChange).AddTo(ref bag);
            
        }

        void OnHpChange(int value)
        {
            int maxHP = entity.StatsSystem.Stats.MaxHP;
            float hpRatio = value * 1.0f / maxHP;
            healthFill.fillAmount = hpRatio;
            healthText.text = value.ToString();
        }
    }

}
