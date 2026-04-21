using Cysharp.Threading.Tasks;
using WEngine.MVP;

namespace GameCore.Presentation.Panel
{
    public class PanelActivityPresenter : ActivityPresenter<PanelActivity, PanelActivityViewState>
    {
       
        public PanelActivityPresenter(PanelActivity view) : base(view)
        {
        
        }

        public UniTask ShowPanel() => View.ShowPanel();


        public UniTask HidePanel() => View.HidePannel();

    }
}