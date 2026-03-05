using System;
using Cysharp.Threading.Tasks;
using WEngine.MVP;

namespace GameCore.Presentation
{
    public class TestModal : Modal<TestViewState>
    {
        public override UniTask InitializeState(TestViewState state, Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }
}