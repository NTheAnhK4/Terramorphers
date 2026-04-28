using GameCore.Domain.Stats;
using JSAM;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Domain.Entity
{
    public class BaseEntityMetadata : ScriptableObject
    {
        [SerializeField] protected string addressable;
        [SerializeField] protected string entityName;
        [SerializeField, PreviewField(height: 50)]
        protected Sprite entityIcon;
        [SerializeField] private EntityStats _entityStats;
        [SerializeField] private SoundFileObject hurtSound;
        [SerializeField] private SoundFileObject deadSound;
        

        public EntityStats EntityStats => _entityStats;


        public SoundFileObject HurtSound => hurtSound;

        public SoundFileObject DeadSound => deadSound;

        public Sprite EntityIcon => entityIcon;

        public string EntityName => entityName;

        public string Addressable => addressable;
    }
}