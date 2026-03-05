using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.Presentation.Shared
{
    public interface ICloseTransition
    {
        UniTask ClosePopup();
    }
}


