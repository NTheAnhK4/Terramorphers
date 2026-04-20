using System.Collections.Generic;
using VitalRouter;

namespace Terramorphers.Command
{
    public class EntityTileDistCommand : ICommand
    {
        public List<ITile> entityTiles { get; set; }
        public string Name { get; set; }
        public ITile Tile { get; set; }
    }
}