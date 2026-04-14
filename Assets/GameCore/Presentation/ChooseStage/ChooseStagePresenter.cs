using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage
{
    public class ChooseStagePresenter : ModalPresenter<ChooseStageModal, ChooseStageViewState>
    {
        [Inject] private IObjectResolver _resolver;
        private LevelMetadata _levelMetadata;
        private int _levelID;
        public ChooseStagePresenter(ChooseStageModal view,int levelID, LevelMetadata levelMetadata) : base(view)
        {
            _levelMetadata = levelMetadata;
            _levelID = levelID;
        }

        protected override UniTask Initialize(Memory<object> args, ChooseStageViewState state, ChooseStageModal view)
        {
            state.LevelName.Value = _levelMetadata.LevelName;
            for (int i = 0; i < _levelMetadata.LevelStageDatas.Count; ++i)
            {
                var levelStageData = _levelMetadata.LevelStageDatas[i];
                var presenter = View.CreateStage(_levelID,i,levelStageData);
                _resolver.Inject(presenter);
                presenter.Initialize();
            }
           
            return UniTask.CompletedTask;
        }
    }
}