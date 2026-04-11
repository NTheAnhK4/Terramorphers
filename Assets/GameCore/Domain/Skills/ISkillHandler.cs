

using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using UnityEngine;

namespace GameCore.Domain.Skill
{
    public interface ISkillHandler
    {
        UniTask Apply<T>(T context, CancellationToken token) where T : class;
        void SetUpContext(Context context);
    }
    public abstract class SkillHandler<T> : ISkillHandler where T : class
    {
        
        protected abstract UniTask Use(T context, CancellationToken token);
        async UniTask ISkillHandler.Apply<T1>(T1 context, CancellationToken token)
        {
            if (context is T tContext)
            {
                await Use(tContext, token);
            }
            else
            {
                Debug.Log($"[Test]Invalid context type. Expected {typeof(T)}, got {typeof(T1)}");
            }
        }

        public abstract void SetUpContext(Context context);
    }
}