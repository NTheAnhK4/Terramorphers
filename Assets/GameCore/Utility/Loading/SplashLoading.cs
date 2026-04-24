using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameCore.Utility.Loading{
    public class SplashLoading : MonoBehaviour
    {
        [SerializeField] private CanvasGroup loadingCanvasGroup;
        [SerializeField] private Image progressFillerImage;
        [SerializeField] private float durationTime;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            LoadGame().Forget();
        }

        private async UniTask LoadGame()
        {
           
            var fillTask = PlayFill();
            var sceneLoadTask = LoadingGamePlayScene();
            var results = await UniTask.WhenAll(fillTask, sceneLoadTask);
            
            if (results.Item2) 
            {
                await PlayFillToFull();
                await FadeAndDestroy();
            }
        }

        private async UniTask<bool> PlayFill()
        {
            var tcs = new UniTaskCompletionSource<bool>();
            progressFillerImage
                .DOFillAmount(0.9f, durationTime)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => tcs.TrySetResult(true));
            return await tcs.Task;
        }

        private async UniTask PlayFillToFull()
        {
            var tcs = new UniTaskCompletionSource();

            progressFillerImage
                .DOFillAmount(1f, 0.3f)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => tcs.TrySetResult());
            await tcs.Task;
        }

        private async UniTask FadeAndDestroy()
        {
            var tcs = new UniTaskCompletionSource();

            loadingCanvasGroup
                .DOFade(0f, 0.5f)
                .OnComplete(() =>
                {
                    tcs.TrySetResult();
                    Destroy(gameObject);
                });
            await tcs.Task;
        }

        private async UniTask<bool> LoadingGamePlayScene()
        {
            var loadSceneHandler = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            await loadSceneHandler;
            return true;
        }

    }
}
