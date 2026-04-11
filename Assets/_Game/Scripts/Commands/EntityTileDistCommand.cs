using VitalRouter;

namespace Terramorphers.Command
{
    public class EntityTileDistCommand : ICommand
    {
        public string Name { get; set; }
        public ITile Tile { get; set; }
    }
}