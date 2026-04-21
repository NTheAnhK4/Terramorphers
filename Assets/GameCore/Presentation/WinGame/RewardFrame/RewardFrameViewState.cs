using GameCore.Domain.Reward;
using R3;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.WinGame
{
    public class RewardFrameViewState : ViewState
    {
        public ReactiveProperty<bool> IsShow { get; } = new();
        public ReactiveProperty<Sprite> RewardSprite { get; } = new();
        public ReactiveProperty<int> Amount { get; } = new();
       
    }
}