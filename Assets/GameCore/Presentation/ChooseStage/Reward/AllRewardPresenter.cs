using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using VContainer;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.ChooseStage.Reward
{
    public class AllRewardPresenter : AppViewPresenter<AllRewardView, AllRewardViewState>
    {
        [Inject] private IObjectResolver _resolver;
        private IReadOnlyList<StageRewardData> _rewardDatas;
        public AllRewardPresenter(AllRewardView view, IReadOnlyList<StageRewardData> rewardDatas) : base(view)
        {
            _rewardDatas = rewardDatas;
        }

        protected override UniTask Initialize(AllRewardViewState state, AllRewardView view)
        {
            state.FinishShow.Subscribe(_ => view.SpawnItem(_rewardDatas, _resolver)).AddTo(view);
       
            return UniTask.CompletedTask;
        }
    }
}