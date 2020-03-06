using System.Drawing;
using System.Linq;

namespace Dungeons.Common
{
    public class FloorSize
    {
        public static readonly FloorSize Small = new FloorSize { Name = "Small", MapSize = new Size(140, 140), NumRows = 4, NumColumns = 4 };
        public static readonly FloorSize Medium = new FloorSize { Name = "Medium", MapSize = new Size(140, 280), NumRows = 8, NumColumns = 4 };
        public static readonly FloorSize Large = new FloorSize { Name = "Large", MapSize = new Size(280, 280), NumRows = 8, NumColumns = 8 };

        public static readonly FloorSize[] Sizes = { Small, Medium, Large };

        public string Name { get; private set; }
        public Size MapSize { get; private set; }
        public int NumRows { get; set; }
        public int NumColumns { get; set; }
        public Size OffsetToTimerMarker { get; set; }
        public int GridOffsetX => (MapSize.Width - NumColumns * MapUtils.RoomSize) / 2;
        public int GridOffsetY => (MapSize.Height - NumRows * MapUtils.RoomSize) / 2;

        public static FloorSize ByName(string name) => Sizes.FirstOrDefault(s => s.Name == name) ?? Small;

        public static FloorSize ByDimensions(int width, int height)
        {
            if (width == 4 && height == 4)
                return Small;
            if (width == 4 && height == 8)
                return Medium;
            return Large;
        }

        public Point ClientToMapCoords(Point p) => new Point((p.X - GridOffsetX) / MapUtils.RoomSize, NumRows - (p.Y - GridOffsetY) / MapUtils.RoomSize - 1);

        // Returns the upper-left corner of the square at p.
        public Point MapToClientCoords(Point p) => new Point(p.X * MapUtils.RoomSize + GridOffsetX, (NumRows - p.Y - 1) * MapUtils.RoomSize + GridOffsetY);

        public bool IsValidMapCoords(Point p) => p.IsInRange(NumColumns, NumRows);
    }
}
