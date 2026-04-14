using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using UnityEngine;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.Lobby
{
    public class LobbyPresenter : ScreenPresenter<LobbyScreen, LobbyViewState>
    {
        private ILevelRepository _levelRepository;
        private ILevelDatabase _levelDatabase;
        private IObjectResolver _resolver;

        [Inject]
        public void Constructor(ILevelRepository levelRepository, IObjectResolver resolver)
        {
            _levelRepository = levelRepository;
            _resolver = resolver;
        }
        public LobbyPresenter(LobbyScreen view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, LobbyViewState state, LobbyScreen view)
        {
            base.Initialize(args, state, view);
            _levelDatabase = _levelRepository.Get();
            
            for (int i = 0; i < _levelDatabase.DataCount; ++i)
            {
                var levelMetadata = _levelDatabase.GetByType(i);
               
                var worldPresenter = View.CreateWorldCell(i,levelMetadata);
                _resolver.Inject(worldPresenter);
                worldPresenter.Initialize();
            }

            state.CurrentIndex.Value = 0;
           
            return UniTask.CompletedTask;
        }
    }
}