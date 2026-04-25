using UnityEngine;

namespace GameCore.Utility.UI
{
    public static class UIUtility
    {
        #region Cache

        private static Camera _cameraCache;
        private static Canvas _canvasCache;
        

        #endregion

        public static Camera Camera
        {
            get
            {
                if (_cameraCache == null) _cameraCache = Camera.main;
                return _cameraCache; 
            }
        }

        public static Canvas Canvas
        {
            get
            {
                if (_canvasCache == null) _canvasCache = Object.FindObjectOfType<Canvas>();
                return _canvasCache;
            }
        }

        public static Vector2 WorldPosToAnchoredPosition(RectTransform  parentRectTransform, Vector3 worldPos)
        {
          
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera, worldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRectTransform, 
                screenPoint, 
                Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera,
                out Vector2 localPoint
            );

            return localPoint;
        }
    }
}