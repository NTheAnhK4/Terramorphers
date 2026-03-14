using VitalRouter;

namespace Terramorphers.Command
{
    public class PlayerMoveToTile : ICommand
    {
        public ITile TargetTile { get; set; }
    }
}