using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation
{
    
    public class TestPresenter : ModalPresenter<TestModal, TestViewState>
    {
        public TestPresenter(TestModal view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, TestViewState state, TestModal view)
        {
            return UniTask.CompletedTask;
        }
    }
}
