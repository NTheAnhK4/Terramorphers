using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.Utility
{
    public static class Retry
    {
        public static async UniTask<T> Do<T>(Func<UniTask<T>> action, Func<T, bool> check, TimeSpan retryInterval,
            int maxAttemptCount = int.MaxValue)
        {
            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                if (attempted > 0)
                {
                    await UniTask.Delay(retryInterval, ignoreTimeScale: true);
                }

                var result = await action.Invoke();
                var isSuccess = check(result);
                if (isSuccess)
                    return result;
            }

            return default;
        }

        public static async UniTask<T> Do<T>(Func<UniTask<T>> action, Func<T, bool> check, TimeSpan retryInterval,
            TimeSpan maxInterval, int maxAttemptCount = int.MaxValue)
        {
            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                if (attempted > 0)
                {
                    var delay = retryInterval * attempted;
                    if (delay > maxInterval)
                        delay = maxInterval;
                    await UniTask.Delay(delay, ignoreTimeScale: true);
                }

                var result = await action.Invoke();
                var isSuccess = check(result);
                if (isSuccess)
                    return result;
            }

            return default;
        }
    }
}