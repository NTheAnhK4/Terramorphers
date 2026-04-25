using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ZBase.UnityScreenNavigator.Core{
    public class SimpleTransitionOtherAnimation : TransitionAnimationBehaviour
    {
        [SerializeField] private bool setupPos = false;
        [SerializeField] private RectTransform targetRect;
        [SerializeField] private float _delay;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private EaseType _easeType = EaseType.QuarticEaseOut;
        [SerializeField] private SheetAlignment _beforeAlignment = SheetAlignment.Center;
        [SerializeField] private Vector3 _beforeScale = Vector3.one;
        [SerializeField] private float _beforeAlpha = 1.0f;
        [SerializeField] private SheetAlignment _afterAlignment = SheetAlignment.Center;
        [SerializeField] private Vector3 _afterScale = Vector3.one;
        [SerializeField] private float _afterAlpha = 1.0f;

        private Vector3 _afterPosition;
        private Vector3 _beforePosition;
        private CanvasGroup _canvasGroup;
        public override float Duration => _delay + _duration;

        public override void Setup()
        {
            _beforePosition = ToPosition(_beforeAlignment, targetRect);

            _afterPosition = ToPosition(_afterAlignment, targetRect);
            if (!targetRect.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup = targetRect.gameObject.AddComponent<CanvasGroup>();
            }

            _canvasGroup = canvasGroup;
        }

        public override void SetTime(float time)
        {
            time = Mathf.Max(0.0f, time - _delay);
            var progress = _duration <= 0.0f ? 1.0f : Mathf.Clamp01(time / _duration);
            progress = Easings.Interpolate(progress, _easeType);
            var position = Vector3.Lerp(_beforePosition, _afterPosition, progress);
            var scale = Vector3.Lerp(_beforeScale, _afterScale, progress);
            var alpha = Mathf.Lerp(_beforeAlpha, _afterAlpha, progress);
            if(setupPos) targetRect.anchoredPosition = position;
            targetRect.localScale = scale;
            _canvasGroup.alpha = alpha;
        }

      
        public Vector3 ToPosition(SheetAlignment self, RectTransform rectTransform)
        {
            var rect = rectTransform.rect;
            var width = rect.width;
            var height = rect.height;
            var z = rectTransform.localPosition.z;

            switch (self)
            {
                case SheetAlignment.Left: return new Vector3(-width, 0, z);
                case SheetAlignment.Top: return new Vector3(0, height, z);
                case SheetAlignment.Right: return new Vector3(width, 0, z);
                case SheetAlignment.Bottom: return new Vector3(0, -height, z);
                case SheetAlignment.Center: return new Vector3(0, 0, z);
            }

            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }
        public void SetParams(
            float? duration = null
            , EaseType? easeType = null
            , SheetAlignment? beforeAlignment = null
            , Vector3? beforeScale = null
            , float? beforeAlpha = null
            , SheetAlignment? afterAlignment = null
            , Vector3? afterScale = null
            , float? afterAlpha = null
        )
        {
            if (duration.HasValue)
            {
                _duration = duration.Value;
            }

            if (easeType.HasValue)
            {
                _easeType = easeType.Value;
            }

            if (beforeAlignment.HasValue)
            {
                _beforeAlignment = beforeAlignment.Value;
            }

            if (beforeScale.HasValue)
            {
                _beforeScale = beforeScale.Value;
            }

            if (beforeAlpha.HasValue)
            {
                _beforeAlpha = beforeAlpha.Value;
            }

            if (afterAlignment.HasValue)
            {
                _afterAlignment = afterAlignment.Value;
            }

            if (afterScale.HasValue)
            {
                _afterScale = afterScale.Value;
            }

            if (afterAlpha.HasValue)
            {
                _afterAlpha = afterAlpha.Value;
            }
        }
    }

}
