

using Cysharp.Threading.Tasks;
using GameCore.Domain.Tile;
using UnityEngine;

namespace Terramorphers
{
    public interface ITileFactory
    {
        UniTask<ITile> CreateTile(ETileType type);
        void SetParent(Transform parent);
        void Despawn(ITile tile);
    }

}
