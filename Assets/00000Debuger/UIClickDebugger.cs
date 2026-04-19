using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIRaycastDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);

            Debug.Log("===== UI RAYCAST =====");

            for (int i = 0; i < results.Count; i++)
            {
                var r = results[i];
                Debug.Log($"[Test]{i}: {r.gameObject.name} | depth: {r.depth} | sortingOrder: {r.sortingOrder}");
            }
        }
    }
}