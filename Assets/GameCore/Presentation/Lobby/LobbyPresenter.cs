using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Presentation.Currency.Gold;
using GameCore.Presentation.Panel;
using GameCore.Presentation.Shared;
using VContainer;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.Lobby
{
    public class LobbyPresenter : ScreenPresenter<LobbyScreen, LobbyViewState>
    {
        private ILevelRepository _levelRepository;
        private ILevelDatabase _levelDatabase;
        private IObjectResolver _resolver;
        private TransitionService _transitionService;
        private PanelActivityPresenter _panelActivityPresenter;

        [Inject]
        public void Constructor(ILevelRepository levelRepository, IObjectResolver resolver,
            TransitionService transitionService)
        {
            _levelRepository = levelRepository;
            _resolver = resolver;
            _transitionService = transitionService;
        }
        public LobbyPresenter(LobbyScreen view, PanelActivityPresenter panelActivityPresenter) : base(view)
        {
            _panelActivityPresenter = panelActivityPresenter;
        }

        protected override UniTask Initialize(Memory<object> args, LobbyViewState state, LobbyScreen view)
        {
            base.Initialize(args, state, view);
            _levelDatabase = _levelRepository.Get();
            state.ShowHeroInfo.Subscribe(_ =>ShowHeroInfo().Forget()).AddTo(view);
            state.SettingCommand.Subscribe(ShowSettingModal).AddTo(view);
            for (int i = 0; i < _levelDatabase.DataCount; ++i)
            {
                var levelMetadata = _levelDatabase.GetByType(i);
               
                var worldPresenter = View.CreateWorldCell(i,levelMetadata);
                _resolver.Inject(worldPresenter);
                worldPresenter.Initialize();
            }

            state.CurrentIndex.Value = 0;
            state.HidePanelCommand.Subscribe(_ => HidePanel().Forget()).AddTo(view);


            var goldPresenter = new GoldPresenter(view.GoldView);
            _resolver.Inject(goldPresenter);
            goldPresenter.Initialize();
            return UniTask.CompletedTask;
        }

        private async UniTask HidePanel()
        {
            if (_panelActivityPresenter == null) return;
            await  _panelActivityPresenter.HidePanel();
            await _transitionService.ShowPanelActivity(false);
        }

        private async UniTask ShowHeroInfo()
        {
            var panelPresenter = await _transitionService.ShowPanelActivity(true);
            await panelPresenter.ShowPanel();
            _transitionService.ShowHeroInfoScreen(panelPresenter).Forget();
        }

        private void ShowSettingModal(Unit _) => _transitionService.ShowSettingModal().Forget();
    }
}