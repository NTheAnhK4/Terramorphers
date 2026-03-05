
using UnityEngine;

namespace CoreGame
{
    public class Singleton<T> : ComponentBehaviour where T : ComponentBehaviour
    {
        protected static T instance;
        public static bool HasInstance => instance != null;

        public static T TryGetInstance() => HasInstance ? instance : null;
        private static bool isQuitting = false;
  
        public static T Instance
        {
            get
            {
                if (isQuitting) return null;
                if (instance == null) instance = FindAnyObjectByType<T>();
              
                return instance;
            }
        }

        protected override void Awake()
        {
            InitializeSingleton();

            base.Awake();
        
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying) return;
            instance = this as T;
        }
        protected virtual void OnApplicationQuit() => isQuitting = true;
    }

}
