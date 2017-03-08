using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    public static class MapUtils
    {
        const int MapOffsetX = 2;
        const int MapOffsetY = 2;
        const int MapWidth = 318;
        const int MapHeight = 310;
        const int GridOffsetX = 29;
        const int GridOffsetY = 27;
        const int SquareSize = 32;

        public static Point ClientToMapCoords(Point p)
        {
            return new Point(1 + (p.X - GridOffsetX) / SquareSize, 8 - (p.Y - GridOffsetY) / SquareSize);
        }

        // Returns the upper-left corner of the square at p.
        public static Point MapToClientCoords(Point p)
        {
            return new Point((p.X - 1) * SquareSize + GridOffsetX, (8 - p.Y) * SquareSize + GridOffsetY);
        }

        public static bool IsValidMapCoords(Point p)
        {
            return p.X >= 1 && p.X <= 8 && p.Y >= 1 && p.Y <= 8;
        }
    }
}
