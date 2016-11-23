using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloorBuilder.Enums
{
    public enum DirectionFlag
    {
        AllowDiagonals,
        NoDiagonals
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum Tile
    {
        Empty,
        Floor,
        Wall,
        Entrance
    }
}
