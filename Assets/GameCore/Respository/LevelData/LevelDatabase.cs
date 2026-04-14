using GameCore.Domain.Level;
using GameCore.Domain.Shared;
using UnityEngine;

namespace GameCore.Respository.LevelData
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Database/LevelDatabase", order = 0)]
    public class LevelDatabase : BaseDatabase<int, LevelMetadata>, ILevelDatabase
    {
        public int DataCount => Datas.Count;
    }
}