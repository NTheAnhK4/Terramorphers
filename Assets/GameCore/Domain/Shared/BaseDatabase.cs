using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Domain.Shared
{
    public abstract class BaseDatabase<TKey, TValue> : ScriptableObject, ISerializationCallbackReceiver
        , IMasterDatabase<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Datas { get; private set; }

        [TableList] [SerializeField] private SerializedElement[] serializedElements;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            //serializedElements = Argument.Select(element => new SerializedElement(element.Key, element.Value)).ToArray();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Datas = new Dictionary<TKey, TValue>();
            if (serializedElements != null)
            {
                Array.ForEach(serializedElements, element => Datas.TryAdd(element.key, element.value));
            }
        }


        public virtual TValue GetByType(TKey type)
        {
            return Datas.GetValueOrDefault(type);
        }

        [Serializable]
        private sealed class SerializedElement
        {
            [HorizontalGroup("row", width:.25f)][HideLabel]
            public TKey key;
            [HorizontalGroup("row", width:.75f)][HideLabel]
            public TValue value;

            public SerializedElement(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }

#if UNITY_EDITOR

        public void SetSerializedElements(Dictionary<TKey, TValue> elements)
        {
            List<SerializedElement> elementsList = new List<SerializedElement>();
            foreach (var element in elements)
            {
                elementsList.Add(new SerializedElement(element.Key, element.Value));
            }

            serializedElements = elementsList.ToArray();
        }
#endif
    }
}