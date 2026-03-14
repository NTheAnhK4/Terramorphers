using VitalRouter;

namespace Terramorphers.Command
{
    public class SetMovableTilesCommand : ICommand
    {
        public ITile CenterTile { get; set; }
        public int Distance { get; set; }
    }
}