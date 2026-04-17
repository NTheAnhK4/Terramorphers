using GameCore.Domain.Quest;
using GameCore.Domain.Shared;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace GameCore.Respository.Quest
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "Database/QuestDatabase")]
    public class QuestDatabase : BaseDatabase<int,QuestMetadata>, IQuestDatabase
    {
        #if UNITY_EDITOR
        [Button]
        private void Bake()
        {
            foreach (var item in Datas)
            {
                item.Value.QuestID = item.Key;
            }
            SetSerializedElements(Datas);
            EditorUtility.SetDirty(this);
        }
        #endif
        public int DataCount => Datas.Count;
    }
}