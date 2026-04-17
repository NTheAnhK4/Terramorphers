using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace GameCore.Presentation.GamePlay
{
    public class DropDownButton : MonoBehaviour
    {
        [SerializeField] private Image pannel;
        [SerializeField] private Button mainBtn;
        [SerializeField] private List<CanvasGroup> canvasGroups;
        private bool isShow = false;
        private void Start()
        {
            mainBtn.onClick.AddListener(OnMainBtnClick);
            Show(false).Forget();
        }

        private void OnDestroy()
        {
            mainBtn.onClick.RemoveAllListeners();
        }

        private void OnMainBtnClick()
        {
            isShow = !isShow;
            Show(isShow).Forget();
        }

        private async UniTask Show(bool isOn)
        {
            mainBtn.interactable = false;
            try
            {
                if (isOn)
                {
                    pannel.enabled = true;
                    foreach (var cv in canvasGroups)
                    {
                        cv.gameObject.SetActive(true);
                        cv.alpha = 0;
                        cv.transform.localScale = .8f * Vector3.one;
                        _ = FadeAndScale(cv, 1, 1, .2f);
                        await UniTask.Delay(80, cancellationToken: this.GetCancellationTokenOnDestroy());
                    }
                }
                else
                {
                    for (int i = canvasGroups.Count - 1; i >= 0; --i)
                    {
                        var cv = canvasGroups[i];
                        await FadeAndScale(cv, 0, .8f, .15f);
                        cv.gameObject.SetActive(false);
                    }

                    pannel.enabled = false;
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                mainBtn.interactable = true;
            }
            
        }
        private async UniTask FadeAndScale(CanvasGroup cg, float targetAlpha, float targetScale, float duration)
        {
            DOTween.Kill(cg);
            var t1 = cg.DOFade(targetAlpha, duration).SetTarget(cg);
            var t2 = cg.transform.DOScale(targetScale, duration).SetTarget(cg);

            await UniTask.WhenAll(t1.ToUniTask(), t2.ToUniTask());
        }
    }
}