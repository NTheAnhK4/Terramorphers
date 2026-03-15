using System;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using VContainer;
using VitalRouter;
using WEngine.MVP;
using R3;
using UnityEngine;

namespace GameCore.Presentation.GamePlay
{
    public class GamePlayPresenter : ScreenPresenter<GamePlayScreen, GamePlayViewState>
    {
        private ICommandPublisher _publisher;
        private ICommandSubscribable _subscribabale;
        private GamePlayViewState _state;
        [Inject]
        public void Constructor(ICommandPublisher publisher, ICommandSubscribable subscribable)
        {
            _publisher = publisher;
            _subscribabale = subscribable;
        }
        public GamePlayPresenter(GamePlayScreen view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, GamePlayViewState state, GamePlayScreen view)
        {
            _state = state;
            _subscribabale.Subscribe<ToggleEndTurnCommand>(ToggleEndTurnButton).AddTo(view);
            _subscribabale.Subscribe<IncreaseRoundCommand>(SetRound).AddTo(view);
            state.EndTurnCommand.Subscribe(OnEndTurnBtnClick).AddTo(view);
            return base.Initialize(args, state, view);
        }

        private void SetRound(IncreaseRoundCommand command, PublishContext context) => _state.CurrentRound.Value = command.NewRound;
        

        private void OnEndTurnBtnClick(Unit _)
        {
            _publisher.PublishAsync(new EndEntityTurnCommand());
            
        }

        private void ToggleEndTurnButton(ToggleEndTurnCommand command, PublishContext context)
        {
            _state.IsActiveEndTurnCommand.Value = command.IsOn;
        }
        
    }
}