using VitalRouter;

namespace Terramorphers.Command
{
    //only used by player
    public class SelectTileCommand : ICommand
    {
        public ITile SelectedTile { get; set; }
    }
}