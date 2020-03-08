using System.Drawing;
using System.Linq;

namespace Dungeons.Common
{
    public class FloorSize
    {
        public static readonly FloorSize Small = new FloorSize { Name = "Small", NumRows = 4, NumColumns = 4, MinRC = 10, MaxRC = 16, MinCrit = 6, MaxCrit = 8, RareRCSpread = 2 };
        public static readonly FloorSize Medium = new FloorSize { Name = "Medium", NumRows = 8, NumColumns = 4, MinRC = 23, MaxRC = 32, MinCrit = 10, MaxCrit = 14, RareRCSpread = 3 };
        public static readonly FloorSize Large = new FloorSize { Name = "Large", NumRows = 8, NumColumns = 8, MinRC = 50, MaxRC = 64, MinCrit = 19, MaxCrit = 23, RareRCSpread = 4 };

        public static readonly FloorSize[] Sizes = { Small, Medium, Large };

        static readonly Size oldImageSize = new Size(318, 310);

        public string Name { get; private set; }
        public int NumRows { get; private set; }
        public int NumColumns { get; private set; }
        public int MinRC { get; private set; }
        public int MaxRC { get; private set; }
        // e.g. 4 for larges, 3 for meds, 2 for smalls.
        public int RareRCSpread { get; private set; }
        public int MinCrit { get; private set; }
        public int MaxCrit { get; private set; }

        public static FloorSize ByName(string name) => Sizes.FirstOrDefault(s => s.Name == name) ?? Small;

        public static FloorSize ByDimensions(int width, int height)
        {
            if (width == 4 && height == 4)
                return Small;
            if (width == 4 && height == 8)
                return Medium;
            return Large;
        }

        public static FloorSize ByImageSize(Size imageSize)
        {
            if (imageSize.Width >= 256 && imageSize.Height >= 256)
                return Large;
            else if (imageSize.Height >= 256)
                return Medium;
            return Small;
        }

        private int GetGridOffsetX(Size imageSize)
        {
            if (imageSize == oldImageSize)
                return 29;

            return (imageSize.Width - NumColumns * MapUtils.RoomSize) / 2;
        }

        private int GetGridOffsetY(Size imageSize)
        {
            if (imageSize == oldImageSize)
                return 27;

            return (imageSize.Height - NumRows * MapUtils.RoomSize) / 2;
        }

        public Point ClientToMapCoords(Point p, Size imageSize) => new Point((p.X - GetGridOffsetX(imageSize)) / MapUtils.RoomSize, NumRows - (p.Y - GetGridOffsetY(imageSize)) / MapUtils.RoomSize - 1);

        // Returns the upper-left corner of the square at p.
        public Point MapToClientCoords(Point p, Size imageSize) => new Point(p.X * MapUtils.RoomSize + GetGridOffsetX(imageSize), (NumRows - p.Y - 1) * MapUtils.RoomSize + GetGridOffsetY(imageSize));

        public bool IsInRange(Point p) => p.IsInRange(NumColumns, NumRows);

        public FloorSize Copy()
        {
            return (FloorSize)MemberwiseClone();
        }
    }
}
