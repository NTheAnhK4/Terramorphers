using System;
using System.Collections.Generic;
using DG.Tweening;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Terramorphers
{
    public class EntityUI : MonoBehaviour
    {
        [SerializeField, TabGroup("Components")] private TerramorphersEntity entity;

        [SerializeField, TabGroup("Health")]
        private Image healthFill;

        [SerializeField, TabGroup("Health")] private TextMeshProUGUI healthText;

        [SerializeField, TabGroup("Noti")] private List<TextMeshProUGUI> statsNotiText = new();
        private Stack<TextMeshProUGUI> statsNotiStacks = new();
     
        
        private DisposableBag bag;
        private Vector2 originalRect;
        [Button]
        public void LoadComponent()
        {
           
            if (entity == null) entity = GetComponentInParent<TerramorphersEntity>();
            if (healthFill == null) healthFill = transform.Find("Health/HealthBar/Mask/Fill").GetComponent<Image>();
            if (healthText == null) healthText = transform.Find("Health/Value").GetComponent<TextMeshProUGUI>();
        }

        protected void Awake()
        {
            originalRect = statsNotiText[0].rectTransform.anchoredPosition;
            foreach (var text in statsNotiText)
            {
                text.gameObject.SetActive(false);
                statsNotiStacks.Push(text);
            }
            entity.OnInitialized += SubscribeEvent;
            if (healthFill.material != null)
                healthFill.material = Instantiate(healthFill.material);
           
        }

       

        private void OnDestroy()
        {
            entity.OnInitialized -= SubscribeEvent;
            bag.Dispose();
        }

        private void SubscribeEvent()
        {
            entity.DataCache.RemainHP.Subscribe(OnHpChange).AddTo(ref bag);
            entity.StatsNoti.Subscribe(ShowStatsNoti).AddTo(ref bag);
        }

        void OnHpChange(int value)
        {
            int maxHP = entity.StatsSystem.Stats.MaxHP;
            float hpRatio = value * 1.0f / maxHP;
            healthFill.material.SetFloat(Health, hpRatio);
            healthText.text = value.ToString();
        }

        private Queue<string> _notiQueue = new();
        private bool _isShowing;
        private static readonly int Health = Shader.PropertyToID("_Health");

        public void ShowStatsNoti(string text)
        {
            _notiQueue.Enqueue(text);

            if (!_isShowing)
            {
                ProcessQueue().Forget();
            }
        }
        private async Cysharp.Threading.Tasks.UniTaskVoid ProcessQueue()
        {
            _isShowing = true;

            while (_notiQueue.Count > 0)
            {
                string text = _notiQueue.Dequeue();

                ShowOne(text); 

                await Cysharp.Threading.Tasks.UniTask.Delay(
                    TimeSpan.FromSeconds(0.45f)
                );
            }

            _isShowing = false;
        }
        void ShowOne(string text)
        {
            if (statsNotiStacks.Count == 0) return;

            var notiText = statsNotiStacks.Pop();
            notiText.text = text;
            notiText.gameObject.SetActive(true);

            var t = notiText.transform;
            var rect = (RectTransform)t;

            Vector2 startPos = rect.anchoredPosition; 

         
            t.localScale = Vector3.zero;
            rect.anchoredPosition = startPos;
            notiText.color = new Color(notiText.color.r, notiText.color.g, notiText.color.b, 1f);

            float up = 1f;

            var seq = DG.Tweening.DOTween.Sequence();

            seq.Append(t.DOScale(1.5f, 0.2f).SetEase(DG.Tweening.Ease.OutBack));
            seq.Append(t.DOScale(1f, 0.15f));

            seq.Append(rect.DOAnchorPosY(startPos.y + up, 0.5f));
            seq.Join(notiText.DOFade(0f, 0.5f)); 

            seq.OnComplete(() =>
            {
               
                rect.anchoredPosition = startPos;
                t.localScale = Vector3.one;
                notiText.color = new Color(notiText.color.r, notiText.color.g, notiText.color.b, 1f);

                notiText.gameObject.SetActive(false);
                statsNotiStacks.Push(notiText); 
            });
        }
       
    }

}
