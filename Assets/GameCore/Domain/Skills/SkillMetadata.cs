using System;

using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using Sirenix.OdinInspector;

using UnityEngine;
namespace GameCore.Domain.Skill{
    [Serializable]
    public class SkillMetadata
    {
        [SerializeField] private int skillID;
        [SerializeField][PreviewField(height:50)] private Sprite skillSprite;
        [SerializeField] private ESkillRarity _eSkillRarity;
        [SerializeField] private string skillName;
        [SerializeField] private int costs;
        
        [SerializeField] private int range;
        [SerializeField] private int areaOfEffect;
        [SerializeField] private int coolDown;
        [SerializeField] private int precoolDown;
        [SerializeField, TextArea] private string description;
        [SerializeField] private List<ESkillTargetType> skillTargetTypes;
        [SerializeReference] private List<ISkillHandler> skillHandlers = new();

        public Sprite SkillSprite => skillSprite;

        public int CoolDown => coolDown;

        public int PrecoolDown => precoolDown;

        public ESkillRarity ESkillRarity => _eSkillRarity;

        public string SkillName => skillName;

        public int SkillCosts => costs;

        public int SkillID => skillID;

        public int AreaOfEffect => areaOfEffect;

        public IReadOnlyList<ESkillTargetType> SkillTargetTypes => skillTargetTypes;

        public string Description => description;

        public int Range => range;

        public async UniTask Apply<TOwner, TTarget>(TOwner owner,TTarget target ,CancellationToken token) where TOwner : class where TTarget : class
        {
            try
            {
                List<UniTask> uniTasks = new();
                foreach (var skillHandler in skillHandlers)
                {
                    uniTasks.Add(skillHandler.Apply(owner,target, token));
                
                }

                await UniTask.WhenAll(uniTasks);
            }
            catch(OperationCanceledException){}
            
        }

        public Context GetContext()
        {
            Context context = new Context();
           
            foreach (var skillHandler in skillHandlers)
            {
                skillHandler.SetUpContext(context);
            }

            return context;
        }
    }

}
