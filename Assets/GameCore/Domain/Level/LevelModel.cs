using System;

namespace GameCore.Domain.Level
{
    [Serializable]
    public class LevelModel 
    {
        public int CurrentLevel { get; }

        public LevelModel(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
        
    }

}
