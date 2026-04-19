
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using R3;
using TMPro;
using UnityEngine;
using WEngine.MVP;
using UnityEngine.UI;
namespace GameCore.Presentation.Skill
{
    public class SkillInfoView : AppView<SkillInfoViewState>
    {
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private Image skillImage;
        [SerializeField] private TextMeshProUGUI targetText;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI manaText;
        [SerializeField] private TextMeshProUGUI coolDownText;
        [SerializeField] private TextMeshProUGUI rangeText;
        [SerializeField] private TextMeshProUGUI areaOfEffectText;
        
        protected override UniTask Initialize(SkillInfoViewState state)
        {
            state.skillMetaData.Subscribe(SetupSkillInfo).AddTo(this);
            state.ShowSkillInfoCommand.Subscribe(Show).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void SetupSkillInfo(SkillMetadata skillMetadata)
        {
            if (skillMetadata == null) return;
            skillNameText.text = skillMetadata.SkillName;
            skillImage.sprite = skillMetadata.SkillSprite;
            targetText.text = "<sprite=5>" + string.Join(",", skillMetadata.SkillTargetTypes);
            description.text = skillMetadata.Description;
            manaText.text = skillMetadata.SkillCosts.ToString();
            coolDownText.text = skillMetadata.CoolDown.ToString();

            rangeText.text = skillMetadata.Range.ToString();
            rangeText.transform.parent.gameObject.SetActive(skillMetadata.Range > 0);

            areaOfEffectText.text = skillMetadata.AreaOfEffect.ToString();
            areaOfEffectText.transform.parent.gameObject.SetActive(skillMetadata.AreaOfEffect > 0);
        }

        private void Show(bool isShow) => gameObject.SetActive(isShow);
    }

}
