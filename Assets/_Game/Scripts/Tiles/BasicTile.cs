using CoreGame;

namespace Terramorphers
{
    public class BasicTile : BaseTile
    {
        public override bool IsPassable() => true;
        public override bool IsBlockVisibility() => false;

        public override int GetMoveCost() => 1;
    }
}