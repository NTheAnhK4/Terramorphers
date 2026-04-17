using System;
using R3;

namespace GameCore.Domain.Level
{
    [Serializable]
    public class LevelModel
    {
        public int CurrentLevel;
        public int SelectedLevel { get; set; }
        public int SelectedStage { get; set; }
        

        public LevelModel(int currentLevel)
        {
            CurrentLevel = currentLevel;
        }
        
    }

}
