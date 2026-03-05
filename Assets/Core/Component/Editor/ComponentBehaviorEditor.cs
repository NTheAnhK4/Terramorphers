#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace  CoreGame
{
    [CustomEditor(typeof(ComponentBehaviour), true)]
    public class ComponentBehaviorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ComponentBehaviour cb = (ComponentBehaviour)target;

            if (GUILayout.Button("LoadComponent"))
            {
                cb.LoadComponent();
            }
        }
    }
}

#endif
