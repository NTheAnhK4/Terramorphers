
using System;
using Cysharp.Threading.Tasks;
using R3;
using Terramorphers;
using UnityEngine;


namespace UtilityAI.DataCache
{
    public class EntityDerivedDataCalculator : IDisposable
    {
        private TerramorphersEntity entity;
        private DisposableBag bag;
        public EntityDerivedDataCalculator(TerramorphersEntity entity)
        {
            this.entity = entity;

            entity.DataCache.RemainMana.Skip(1).Subscribe(OnManaChange).AddTo(ref bag);
            entity.DataCache.RemainStamina.Skip(1).Subscribe(OnStaminaChange).AddTo(ref bag);
            entity.DataCache.RemainHP.Skip(1).Subscribe(OnHPChange).AddTo(ref bag);
        }

        void OnManaChange(int value)
        {
            
            int totalMana = entity.StatsSystem.Stats.Mana;
            float manaAvailabilityRatio;
            if (totalMana == 0)
            {
                manaAvailabilityRatio = 0;
            }
            else
            {
                manaAvailabilityRatio = 1.0f * value / totalMana;
            }
            entity.Context.SetData(BlackBoardConstant.REMAIN_MANA_KEY, value);
            entity.Context.SetData(BlackBoardConstant.MANA_AVAILABILITY_RATIO,manaAvailabilityRatio);
        }

        void OnStaminaChange(int value)
        {
            int totalStamina = entity.StatsSystem.Stats.Stamina;
            float staminaAvailability;
            if (totalStamina == 0)
            {
                staminaAvailability = 0;
            }
            else
            {
                staminaAvailability = 1.0f * value/ totalStamina;
            }
            entity.Context.SetData(BlackBoardConstant.REMAIN_STAMINA_KEY, value);
            entity.Context.SetData(BlackBoardConstant.STAMINA_AVAILABILITY_RATIO, staminaAvailability);
        }

        void OnHPChange(int value)
        {
            int maxHP = entity.StatsSystem.Stats.MaxHP;
           
            entity.Context.SetData(string.Format(BlackBoardConstant.ENTITY_HEALTH_FULLNESS_RATIO, entity.Name),1.0f * value/maxHP);
        }
        #region Public Func

        public void OnTargetChange(TerramorphersEntity target)
        {
            int remainStamina = entity.DataCache.RemainStamina.Value;
            float entityNearness;
            if (remainStamina == 0)
            {
                entityNearness = 0;
            }
            else
            {
                entityNearness = Mathf.Clamp01(1 - entity.CurrentTile.Context.GetData<int>(string.Format(BlackBoardConstant.ENTITY_TO_TILE_DISTANCE_KEY, target.Name))
                    * 1.0f / remainStamina);
            }
            entity.Context.SetData(
                string.Format(BlackBoardConstant.ENTITY_NEARNESS_RATIO, target.Name), entityNearness);

        }

        #endregion


        public void Dispose()
        {
            bag.Dispose();
        }
    }
}