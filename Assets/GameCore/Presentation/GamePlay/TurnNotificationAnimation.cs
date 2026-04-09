using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameCore.Presentation.GamePlay
{
    public class TurnNotificationAnimation : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform background;
        [SerializeField] private TextMeshProUGUI text;

        [Button]
        public void Show()
        {
           
    
            background.transform.localScale = Vector3.one;

            var seq = DOTween.Sequence();


            var color = text.color;
            color.a = 0;
            text.color = color;

            text.transform.localScale = Vector3.zero;
            text.rectTransform.anchoredPosition = new Vector2(-1200, 0);


            seq.Join(holder.DOScaleX(1, 0.4f).SetEase(Ease.OutQuad));

            seq.Join(
                text.rectTransform.DOAnchorPosX(0, 0.6f)
                    .SetEase(Ease.OutCubic)
            );

            seq.Join(text.DOFade(1, 0.3f));

            seq.Join(
                text.transform.DOScale(1, 0.4f)
                    .SetEase(Ease.OutBack)
            );


            seq.AppendInterval(1.5f); 


            seq.Append(
                text.rectTransform.DOAnchorPosX(1200, 0.3f)
                    .SetEase(Ease.InCubic)
            );

            seq.Join(
                text.transform.DOScale(0, 0.25f)
                    .SetEase(Ease.InBack)
            );


            seq.Join(
                background.DOScaleX(0, 0.3f)
                    .SetEase(Ease.InQuad)
            );

            seq.Play();
            
        }
    }

}
