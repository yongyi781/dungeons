using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MapGenerator
{
    public static class MapUtils
    {
        public static readonly Point Invalid = new Point(-1, -1);

        public static readonly Size[] Offsets = { new Size(-1, 0), new Size(1, 0), new Size(0, -1), new Size(0, 1) };
        public static readonly Direction[] Directions = { Direction.W, Direction.E, Direction.S, Direction.N };
        public static readonly Dictionary<Size, RoomType> OffsetRoomTypeDict = new Dictionary<Size, RoomType>
        {
            [new Size(-1, 0)] = RoomType.W,
            [new Size(1, 0)] = RoomType.E,
            [new Size(0, -1)] = RoomType.S,
            [new Size(0, 1)] = RoomType.N
        };

        public static bool IsInRange(Point p) => p.X >= 0 && p.X < 8 && p.Y >= 0 && p.Y < 8;

        public static Point Add(this Point p, Direction dir)
        {
            if (dir == Direction.None)
                return Invalid;
            return p + Offsets[(int)dir];
        }

        public static Direction Flip(Direction dir)
        {
            switch (dir)
            {
                case Direction.W:
                    return Direction.E;
                case Direction.E:
                    return Direction.W;
                case Direction.S:
                    return Direction.N;
                case Direction.N:
                    return Direction.S;
                default:
                    return Direction.None;
            }
        }

        public static RoomType ToRoomType(Direction dir)
        {
            switch (dir)
            {
                case Direction.W:
                    return RoomType.W;
                case Direction.E:
                    return RoomType.E;
                case Direction.S:
                    return RoomType.S;
                case Direction.N:
                    return RoomType.N;
                default:
                    return RoomType.NotOpened;
            }
        }

        public static string GetRoomTypeResourceString(RoomType type)
        {
            if ((int)type <= 0)
                return "NotOpened";

            var str = "Room";
            if ((type & RoomType.E) != 0)
                str += "E";
            if ((type & RoomType.N) != 0)
                str += "N";
            if ((type & RoomType.S) != 0)
                str += "S";
            if ((type & RoomType.W) != 0)
                str += "W";
            return str;
        }

        public static string ToPrettyString(this Direction[,] dirs)
        {
            var sb = new StringBuilder();
            for (int y = dirs.GetLength(0) - 1; y >= 0; y--)
            {
                for (int x = 0; x < dirs.GetLength(1); x++)
                {
                    var a = dirs[x, y];
                    sb.Append(a == Direction.None ? "-" : a.ToString());
                }
                if (y > 0)
                    sb.AppendLine();
            }
            return sb.ToString();
        }

        public static Bitmap GetRoomTypeBitmap(RoomType type)
        {
            return (Bitmap)Resources.ResourceManager.GetObject(MapUtils.GetRoomTypeResourceString(type));
        }
    }
}
