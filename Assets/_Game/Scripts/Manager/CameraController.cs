using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terramorphers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private Vector3 topRight = new Vector3(13.5f, 7.5f, 0);
        [SerializeField] private Vector3 bottomLeft = new Vector3(-13.5f, -7.5f, 0);
        private void Start() => Fit();
        private void Fit()
        {
            if(mainCam == null) mainCam = Camera.main;
            if (mainCam == null) return;
            Vector2 size = topRight - bottomLeft;

            float aspect = (float)Screen.width / Screen.height;


            float sizeByHeight = size.y / 2f;

            float sizeByWidth = (size.x / aspect) / 2f;

            float orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
            mainCam.orthographicSize = orthographicSize;
            Vector3 center = (topRight + bottomLeft) / 2f;

            mainCam.transform.position = new Vector3(center.x, center.y, mainCam.transform.position.z);

        }
        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Vector3 size = topRight - bottomLeft;
            Vector3 center = (topRight + bottomLeft) / 2f;

            Gizmos.DrawWireCube(center, size);
        }
        #endif

    }

}
