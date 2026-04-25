using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terramorphers
{
    public class MountainTile : BaseTile
    {
        public override bool IsPassable() => false;

        public override bool IsBlockVisibility() => true;

        public override int GetMoveCost() => 0;
       
    }

}
