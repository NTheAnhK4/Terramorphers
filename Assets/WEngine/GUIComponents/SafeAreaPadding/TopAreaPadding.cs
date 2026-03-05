using UnityEngine;

namespace WEngine.GUIComponents.SafeAreaPadding
{
    [RequireComponent(typeof(RectTransform))]
    public class TopAreaPadding : MonoBehaviour//, ILayoutGroup
    {
        private RectTransform _rectTransform;
        
        private void Awake()
        {
            AdjustSizeForSafeArea();
        }

        private void AdjustSizeForSafeArea()
        {
            _rectTransform = GetComponent<RectTransform>();
            var screenRect = Rect.MinMaxRect(0, 0, Screen.width, Screen.height);
            var safeArea = Screen.safeArea;
            var sizeDelta = _rectTransform.sizeDelta;
            sizeDelta.y = Mathf.Max(0, sizeDelta.y - Mathf.Max(0, screenRect.yMax - safeArea.yMax));
            _rectTransform.sizeDelta = sizeDelta;
        }

        public void SetLayoutHorizontal()
        {
        }

        public void SetLayoutVertical()
        {
            AdjustSizeForSafeArea();
        }
    }
}