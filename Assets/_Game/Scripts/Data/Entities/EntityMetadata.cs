using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terramorphers
{
    [Serializable]
    public class EntityMetadata
    {
        [SerializeField] private string addressable;

        public string Addressable => addressable;
    }

}
