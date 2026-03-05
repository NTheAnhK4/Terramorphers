using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    public class PersistentSingleton<T> : ComponentBehaviour where T : ComponentBehaviour
    {
        public bool AutoUnparentOnAwake = true;
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
        
            if(AutoUnparentOnAwake) transform.SetParent(null);
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if(instance != this) Destroy(gameObject);
            }
       
        }

        protected virtual void OnApplicationQuit() => isQuitting = true;
    }

}
