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
        public void SetCurrentStageOfLevel(int level, int value) => _apiGateway.SetCurrentStageOfLevel(level, value);
    }
}