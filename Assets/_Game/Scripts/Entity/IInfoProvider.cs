using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;
namespace Terramorphers
{
    public interface IInfoProvider
    {
        ReactiveProperty<bool> IsShowInfo { get; }
       
    }

}
