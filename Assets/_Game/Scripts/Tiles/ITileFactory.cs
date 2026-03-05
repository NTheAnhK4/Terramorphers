

using Terramorphers;
using UnityEngine;

public interface ITileFactory
{
     ITile Create(ETileType type);
     void SetParent(Transform parent);
}
