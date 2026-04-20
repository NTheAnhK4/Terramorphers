using GameCore.APIGateway.Level;
using GameCore.Domain.Level;

namespace GameCore.Usecase.Level
{
    public class LevelUseCase : BaseUseCase<LevelAPIGateway, LevelModel>
    {
        public LevelUseCase(LevelAPIGateway apiGateway)
        {
            _apiGateway = apiGateway;
        }

        public int GetCurrentStageOfLevel(int level) => _apiGateway.GetCurrentStageOfLevel(level);
        public void SetCurrentStageOfLevel(int level, int value)
        {
            int currentStageOfLevel = GetCurrentStageOfLevel(level);
            if (currentStageOfLevel >= value) return;
            _apiGateway.SetCurrentStageOfLevel(level, value);
        }

        public int GetStars(int level, int stage) => _apiGateway.GetStars(level, stage);
        public void SetStars(int level, int stage, int value)
        {
            int previousStar = GetStars(level, stage);
            if (previousStar >= value) return;
            _apiGateway.SetStars(level, stage, value);
        }
    }
}