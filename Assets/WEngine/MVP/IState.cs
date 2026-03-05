using System;
using Cysharp.Threading.Tasks;

namespace WEngine.MVP
{
    public interface IState<in TViewState> where TViewState : ViewState
    {
        void SetupState(TViewState state);
        UniTask InitializeState(TViewState state, Memory<object> args);
        UniTask WillPushEnterState(TViewState state, Memory<object> args);
    }
}