

using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using JSAM;
using UnityEngine;

namespace GameCore.Domain.Skill
{
    public interface ISkillHandler
    {
        UniTask Apply<TOwner, TTarget>(TOwner owner, TTarget target, CancellationToken token) where TOwner : class where TTarget:class;
        void SetUpContext(Context context);
    }
    public abstract class SkillHandler<TOwner, TTarget> : ISkillHandler
        where TOwner : class where TTarget : class
    {
        [SerializeField] private SoundFileObject sound;
        [SerializeField] protected float delayTime;
        protected abstract UniTask Use(TOwner owner,TTarget target, CancellationToken token);
       

        public async UniTask Apply<TOwner1, TTarget1>(TOwner1 owner, TTarget1 target, CancellationToken token) where TOwner1 : class where TTarget1 : class
        {
            if (owner is TOwner towner && target is TTarget ttarget)
            {
                if (sound != null) AudioManager.PlaySound(sound);
                await Use(towner, ttarget, token);
            }
          
        }

        public abstract void SetUpContext(Context context);
    }
}