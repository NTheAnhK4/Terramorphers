using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoreGame
{
    public class ComponentBehaviour : MonoBehaviour
    {
        public virtual void LoadComponent()
        {
        
        }

        protected virtual void Reset()
        {
            LoadComponent();
        }

        protected virtual void Awake()
        {
            LoadComponent();
        }

   
    }

}
