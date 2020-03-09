using System;
using System.Drawing;

namespace Dungeons.Common
{
    public class FloorSize
    {
        public static readonly FloorSize Small = new FloorSize { NumRows = 4, NumColumns = 4 };
        public static readonly FloorSize Medium = new FloorSize { NumRows = 8, NumColumns = 4 };
        public static readonly FloorSize Large = new FloorSize { NumRows = 8, NumColumns = 8 };

        public static readonly FloorSize[] RSSizes = { Small, Medium, Large };

        static readonly Size oldImageSize = new Size(318, 310);

        public int NumRows { get; private set; }
        public int NumColumns { get; private set; }
        public int CritRange => (Math.Max(NumRows, NumColumns) + 1) / 2;
        public int MaxRC => NumRows * NumColumns;
        public int MinRC => 5 * (MaxRC - 4) / 6;
        // e.g. 4 for larges, 3 for meds, 2 for smalls.
        public int RareRCSpread => (MaxRC - MinRC) / 3;
        public int MinCrit => 7 * (MaxRC + 6) / 25;
        public int MaxCrit => MinCrit + CritRange;
        public string Name => (NumColumns, NumRows) switch
        {
            (4, 4) => "Small",
            (4, 8) => "Medium",
            (8, 8) => "Large",
            _ => $"{NumColumns}x{NumRows}"
        };

        public static FloorSize ByDimensions(int width, int height) => (width, height) switch
        {
            (4, 4) => Small,
            (4, 8) => Medium,
            (8, 8) => Large,
            (var x, var y) => new FloorSize { NumRows = x, NumColumns = y }
        };

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
    }
}
