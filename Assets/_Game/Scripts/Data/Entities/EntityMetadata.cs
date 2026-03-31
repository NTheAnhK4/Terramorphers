using System;

using Terramorphers.Stats;
using UnityEngine;

namespace Terramorphers
{
    [Serializable]
    public class EntityMetadata
    {
        [SerializeField] private string addressable;
        [SerializeField] private EntityStats _entityStats;

        public string Addressable => addressable;

        public EntityStats EntityStats => _entityStats;
    }

}
