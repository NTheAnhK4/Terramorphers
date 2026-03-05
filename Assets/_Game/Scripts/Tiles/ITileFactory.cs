

using Cysharp.Threading.Tasks;


using UnityEngine;

namespace Terramorphers
{
    public interface ITileFactory
    {
        UniTask<ITile> CreateTile(ETileType type);
        void SetParent(Transform parent);
    }

}
