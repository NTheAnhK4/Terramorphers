using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using VContainer;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.ChooseStage
{
    public class ChooseStagePresenter : ModalPresenter<ChooseStageModal, ChooseStageViewState>
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private TransitionService _transitionService;
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
            state.OnClose.Subscribe(OnClose).AddTo(View);
            for (int i = 0; i < _levelMetadata.LevelStageDatas.Count; ++i)
            {
                var levelStageData = _levelMetadata.LevelStageDatas[i];
                var presenter = View.CreateStage(_levelID,i,levelStageData, state.OnClose);
                _resolver.Inject(presenter);
                presenter.Initialize();
            }
           
            return UniTask.CompletedTask;
        }

        private void OnClose(Unit _)
        {
            _transitionService.ClosePopup();
        }
    }
}