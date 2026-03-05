using UnityEngine;

namespace GameCore.Utility.Common
{
    public class LevelSize : MonoBehaviour
    {
        public float width = 20f;
        public float height = 27f;
        public Color color = Color.green;

        void OnDrawGizmos()
        {
            Gizmos.color = color;
            Vector3 pos = transform.position;

            Vector3 topLeft = pos + new Vector3(-width / 2, height / 2, 0);
            Vector3 topRight = pos + new Vector3(width / 2, height / 2, 0);
            Vector3 bottomLeft = pos + new Vector3(-width / 2, -height / 2, 0);
            Vector3 bottomRight = pos + new Vector3(width / 2, -height / 2, 0);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}