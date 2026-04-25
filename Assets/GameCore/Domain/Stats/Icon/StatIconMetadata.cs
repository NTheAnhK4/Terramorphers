using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Domain.Stats.Icon
{
    [Serializable]
    public class StatIconMetadata
    {
        public int SpriteIndex;
        [PreviewField(height: 50)] public Sprite Sprite;
    }
}