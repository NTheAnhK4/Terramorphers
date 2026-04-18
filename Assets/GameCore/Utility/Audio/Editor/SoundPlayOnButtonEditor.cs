using JSAM.JSAMEditor;
using UnityEditor;
using UnityEngine;

namespace GameCore.Utility.Audio.Editor
{
    [CustomEditor(typeof(SoundPlayOnButton))]
    [CanEditMultipleObjects]
    public class SoundPlayOnButtonEditor : BaseSoundEditor
    {
        SoundPlayOnButton myScript;


        protected override void Setup()
        {
            base.Setup();

            myScript = (SoundPlayOnButton)target;
        }

        public override void OnInspectorGUI()
        {
            if (myScript == null) return;

            serializedObject.Update();

            DrawAudioProperty();

            EditorGUILayout.Space();

            GUIContent lontent =
                new GUIContent("Audio Player Settings", "Modify settings specific to the Audio Player");
            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}