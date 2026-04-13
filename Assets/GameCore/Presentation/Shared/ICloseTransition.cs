
using Cysharp.Threading.Tasks;


namespace GameCore.Presentation.Shared
{
    public interface ICloseTransition
    {
        UniTask ClosePopup();
    }
}


