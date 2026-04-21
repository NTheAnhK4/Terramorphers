using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;

namespace GameCore.Presentation.Panel
{
    public class PanelActivity : Activity<PanelActivityViewState>
    {
        [SerializeField] private Image pannel;
        public override UniTask InitializeState(PanelActivityViewState state, Memory<object> args)
        {
          
            return UniTask.CompletedTask;
        }

        public async UniTask ShowPanel()
        {
            var color = pannel.color;
            color.a = 0;
            pannel.color = color;
            await pannel.DOFade(1, .5f).AwaitForComplete();
        }

        public async UniTask HidePannel()
        {
            var color = pannel.color;
            color.a = 1;
            pannel.color = color;
            await  pannel.DOFade(0, 1f).AwaitForComplete();
        }
    }

 
}