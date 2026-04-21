using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Domain.Reward;
using GameCore.Domain.Skill;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.WinGame
{
    public class RewardFramePresenter : AppViewPresenter<RewardFrameView, RewardFrameViewState>
    {
        private StageRewardItem _stageRewardItem;
        private IRewardItemRepository _rewardItemRepository;
        private ISkillRepository _skillRepository;

        [Inject]
        public void Constructor(IRewardItemRepository rewardItemRepository, ISkillRepository skillRepository)
        {
            _rewardItemRepository = rewardItemRepository;
            _skillRepository = skillRepository;
        }
        public RewardFramePresenter(RewardFrameView view, StageRewardItem stageRewardItem) : base(view)
        {
            _stageRewardItem = stageRewardItem;
        }

        protected override UniTask Initialize(RewardFrameViewState state, RewardFrameView view)
        {
            state.IsShow.Value = _stageRewardItem != null;
            if (_stageRewardItem != null)
            {
                state.Amount.Value = _stageRewardItem.Amount;
                if (_stageRewardItem.RewardItemType == ERewardItemType.Skill)
                {
                    var skillDatabase = _skillRepository.Get();
                    state.RewardSprite.Value = skillDatabase.GetByType(_stageRewardItem.SkillID).SkillSprite;
                }
                else
                {
                    var rewardItemDatabase = _rewardItemRepository.Get();
                    state.RewardSprite.Value = rewardItemDatabase.GetByType(_stageRewardItem.RewardItemType).RewardSprite;
                }
            }
            return UniTask.CompletedTask;
            
        }
    }
}