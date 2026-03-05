using UnityEngine;

namespace GameCore.Utility
{
    public sealed class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        public static bool HasInstance => _instance != null;
        public static T Current => _instance;

        private static T _instance;
        private bool _enabled;

        /// <summary>
        /// Singleton design pattern
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var obj = new GameObject();
                        obj.name = typeof(T).Name + "_AutoCreated";
                        _instance = obj.AddComponent<T>();
                        _instance.GetComponent<MonoSingleton<T>>().OnStart();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// On start, we check if there's already a copy of the object in the scene. If there's one, we destroy it.
        /// </summary>
        private void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (_instance == null)
            {
                //If I am the first instance, make me the Singleton
                _instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                _enabled = true;

                OnStart();
            }
            else
            {
                //If a Singleton already exists and you find
                //another reference in scene, destroy it!
                if (this != _instance)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnStart()
        {

        }
    }
}