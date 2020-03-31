using System;
using System.Drawing;

namespace Dungeons.Common
{
    public struct FloorSize : IEquatable<FloorSize>
    {
        public static readonly FloorSize Small = new FloorSize { Width = 4, Height = 4  };
        public static readonly FloorSize Medium = new FloorSize { Width = 4, Height = 8 };
        public static readonly FloorSize Large = new FloorSize { Width = 8, Height = 8 };

        public static readonly FloorSize[] RSSizes = { Small, Medium, Large };

        static readonly Size oldImageSize = new Size(318, 310);

        public FloorSize(int width, int height)
        {
            Width = width;
            Height = height;
            MinRC = (width, height) switch
            {
                (4, 4) => 10,
                (4, 8) => 23,
                (8, 8) => 50,
                _ => (int)(Width * Height - 0.57 * Math.Pow(Width * Height, 0.7925))
            };
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int MinRC { get; }
        public Size Size => new Size(Width, Height);
        public int CritRange => (Math.Max(Width, Height) + 1) / 2;
        public int MaxRC => Width * Height;
        // 0.7925 is approximately 0.5*log_2(3). Minimum allowed rc, before gaps almost surely disconnect the map.
        // Experimentally, the 50% threshold for probability that the gaps disconnect the map is at 0.588 * (#rooms)^0.7925.
        // If we set gaps to (#rooms)^0.7925, the number of attempts to generate a valid set of gaps will be around 200 on average.
        public int MinAllowedRC => (int)(Width * Height - Math.Pow(Width * Height, 0.7925));
        // e.g. 4 for larges, 3 for meds, 2 for smalls.
        public int RareRCSpread => (MaxRC - MinRC) / 3;
        public int MinCrit => Math.Max(3, 7 * (MaxRC + 6) / 25);
        public int MaxCrit => MinCrit + CritRange;

        public static bool operator ==(FloorSize left, FloorSize right) => left.Equals(right);
        public static bool operator !=(FloorSize left, FloorSize right) => !(left == right);

        public static FloorSize ByImageSize(Size imageSize)
        {
            if (imageSize.Width >= 256 && imageSize.Height >= 256)
                return Large;
            else if (imageSize.Height >= 256)
                return Medium;
            return Small;
        }

        public bool Equals(FloorSize other) => Width == other.Width && Height == other.Height && MinRC == other.MinRC;

        public Point ClientToMapCoords(Point p, Size imageSize) => new Point((p.X - GetGridOffsetX(imageSize)) / MapUtils.RoomSize, Height - (p.Y - GetGridOffsetY(imageSize)) / MapUtils.RoomSize - 1);

        // Returns the upper-left corner of the square at p.
        public Point MapToClientCoords(Point p, Size imageSize) => new Point(p.X * MapUtils.RoomSize + GetGridOffsetX(imageSize), (Height - p.Y - 1) * MapUtils.RoomSize + GetGridOffsetY(imageSize));

        public bool IsInRange(Point p) => p.IsInRange(Width, Height);

        private int GetGridOffsetX(Size imageSize)
        {
            if (imageSize == oldImageSize)
                return 29;

            return (imageSize.Width - Width * MapUtils.RoomSize) / 2;
        }

        private int GetGridOffsetY(Size imageSize)
        {
            if (imageSize == oldImageSize)
                return 27;

            return (imageSize.Height - Height * MapUtils.RoomSize) / 2;
        }

        public override int GetHashCode()
        {
            if (Height == 0 && Width == 0)
            {
                return 0;
            }
            return Height.GetHashCode() ^ Width.GetHashCode();
        }

        public override bool Equals(object obj) => obj != null && Equals((FloorSize)obj);

        public override string ToString() => (Width, Height) switch
        {
            (4, 4) => "Small",
            (4, 8) => "Medium",
            (8, 8) => "Large",
            (12, 12) => "Huge",
            (16, 16) => "Gigantic",
            _ => $"{Width}x{Height}"
        };
    }
}
