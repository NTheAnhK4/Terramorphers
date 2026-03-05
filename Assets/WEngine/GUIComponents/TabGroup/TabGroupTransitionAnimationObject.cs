using System;
using UnityEngine;
using ZBase.UnityScreenNavigator.Core;

namespace WEngine.GUIComponents.TabGroup
{
    [CreateAssetMenu(fileName = "TabGroupTransitionAnimationObject",
        menuName = "Screen Navigator/Animations/TabGroupTransitionAnimationObject")]
    public class TabGroupTransitionAnimationObject : TransitionAnimationObject
    {
        [SerializeField] private float duration = 0.2f;
        public bool isEnterAnimation;
        public EaseType easeType = EaseType.QuarticEaseOut;

        private Vector3 _afterPosition;
        private Vector3 _beforePosition;
        private int _partnerTabIndex;

        private int _tabIndex;

        public override float Duration => duration;

        public override void SetTime(float time)
        {
            var progress = duration <= 0.0f ? 1.0f : Mathf.Clamp01(time / duration);
            progress = Easings.Interpolate(progress, easeType);
            var position = Vector3.Lerp(_beforePosition, _afterPosition, progress);
            RectTransform.anchoredPosition = position;
        }

        public override void Setup()
        {
            var sheet = RectTransform.GetComponent<ITabContent>();
            var partnerSheet = PartnerRectTransform.GetComponent<ITabContent>();
            _tabIndex = sheet.TabIndex;
            _partnerTabIndex = partnerSheet.TabIndex;

            SheetAlignment beforeAlignment;
            SheetAlignment afterAlignment;
            if (isEnterAnimation)
            {
                beforeAlignment = _tabIndex < _partnerTabIndex ? SheetAlignment.Left : SheetAlignment.Right;
                afterAlignment = SheetAlignment.Center;
            }
            else
            {
                beforeAlignment = SheetAlignment.Center;
                afterAlignment = _tabIndex < _partnerTabIndex ? SheetAlignment.Left : SheetAlignment.Right;
            }

            _beforePosition = ToPosition(beforeAlignment, RectTransform);
            _afterPosition = ToPosition(afterAlignment, RectTransform);
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
    }
}