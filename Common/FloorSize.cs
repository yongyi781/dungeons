using System;
using System.Drawing;
using System.Linq;

namespace Dungeons.Common
{
    public class FloorSize
    {
        public static readonly FloorSize Small = new FloorSize { NumRows = 4, NumColumns = 4, CritRange = 2 };
        public static readonly FloorSize Medium = new FloorSize { NumRows = 8, NumColumns = 4, CritRange = 4 };
        public static readonly FloorSize Large = new FloorSize { NumRows = 8, NumColumns = 8, CritRange = 4 };
        public static readonly FloorSize Larger = new FloorSize { NumRows = 9, NumColumns = 9, CritRange = 5 };
        public static readonly FloorSize Huge = new FloorSize { NumRows = 10, NumColumns = 10, CritRange = 6 };
        public static readonly FloorSize Massive = new FloorSize { NumRows = 11, NumColumns = 11, CritRange = 7 };
        public static readonly FloorSize Gigantic = new FloorSize { NumRows = 12, NumColumns = 12, CritRange = 8 };
        public static readonly FloorSize Colossal = new FloorSize { NumRows = 13, NumColumns = 13, CritRange = 9 };
        public static readonly FloorSize Enormous = new FloorSize { NumRows = 14, NumColumns = 14, CritRange = 10 };
        public static readonly FloorSize Stupendous = new FloorSize { NumRows = 15, NumColumns = 15, CritRange = 11 };
        public static readonly FloorSize Humongous = new FloorSize { NumRows = 16, NumColumns = 16, CritRange = 12 };
        public static readonly FloorSize Gargantuan = new FloorSize { NumRows = 20, NumColumns = 20, CritRange = 15 };

        public static readonly FloorSize[] RSSizes = { Small, Medium, Large };
        public static readonly FloorSize[] ExtraSizes = { Larger, Huge, Massive, Gigantic, Colossal, Enormous, Stupendous, Humongous, Gargantuan };

        static readonly Size oldImageSize = new Size(318, 310);

        public int NumRows { get; private set; }
        public int NumColumns { get; private set; }
        public int CritRange { get; private set; }
        public int MaxRC => NumRows * NumColumns;
        public int MinRC => 5 * (MaxRC - 4) / 6;
        // e.g. 4 for larges, 3 for meds, 2 for smalls.
        public int RareRCSpread => (MaxRC - MinRC) / 3;
        public int MinCrit => 7 * (MaxRC + 6) / 25;
        public int MaxCrit => MinCrit + CritRange;

        public static FloorSize ByDimensions(int width, int height) => (width, height) switch
        {
            (4, 4) => Small,
            (4, 8) => Medium,
            (8, 8) => Large,
            (20, 20) => Gargantuan,
            (var x, var y) when x == y && x >= 9 && x < 9 + ExtraSizes.Length => ExtraSizes[x - 9],
            _ => Large
        };

        public static FloorSize ByImageSize(Size imageSize)
        {
            if (imageSize.Width >= 256 && imageSize.Height >= 256)
                return Large;
            else if (imageSize.Height >= 256)
                return Medium;
            return Small;
        }

        public string GetName() => (NumColumns, NumRows) switch
        {
            (4, 4) => "Small",
            (4, 8) => "Medium",
            (8, 8) => "Large",
            _ => "Other"
        };

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
