using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreGame
{
    public static class GameObjectExtensions
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (!component) component = gameObject.AddComponent<T>();
            return component;
        }

        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
        public static void DestroyChildren(this GameObject gameObject) => gameObject.transform.DestroyChildren();
    }

}
