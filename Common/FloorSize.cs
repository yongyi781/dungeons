using System;
using System.Drawing;

namespace Dungeons.Common
{
    public struct FloorSize : IEquatable<FloorSize>
    {
        public static readonly FloorSize Small = new FloorSize { Height = 4, Width = 4 };
        public static readonly FloorSize Medium = new FloorSize { Height = 8, Width = 4 };
        public static readonly FloorSize Large = new FloorSize { Height = 8, Width = 8 };

        public static readonly FloorSize[] RSSizes = { Small, Medium, Large };

        static readonly Size oldImageSize = new Size(318, 310);

        public FloorSize(int width, int height)
        {
            Height = height;
            Width = width;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Size Size => new Size(Width, Height);
        public int CritRange => (Math.Max(Width, Height) + 1) / 2;
        public int MaxRC => Width * Height;
        public int MinRC => Math.Max(2, 5 * (MaxRC - 4) / 6);
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

        public bool Equals(FloorSize other) => Height == other.Height && Width == other.Width;

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
