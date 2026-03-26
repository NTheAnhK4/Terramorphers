using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terramorphers
{
    public class SandTile : BaseTile
    {
        public override bool IsPassable() => true;

        public override bool IsBlockVisibility() => true;

        public override int GetMoveCost() => 2;
    }

}
