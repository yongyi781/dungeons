using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dungeons.Common
{
    public static class MapUtils
    {
        // Number of pixels per room.
        public const int RoomSize = 32;

        public static readonly Point Invalid = new Point(-1, -1);

        public static readonly Size[] Offsets = { new Size(-1, 0), new Size(1, 0), new Size(0, -1), new Size(0, 1) };
        public static readonly Direction[] Directions = { Direction.W, Direction.E, Direction.S, Direction.N };

        private static readonly Random random = new Random();

        // [0..8) x [0..8)
        public static IEnumerable<Point> GridPoints(int width, int height) =>
            from y in Enumerable.Range(0, height) from x in Enumerable.Range(0, width) select new Point(x, y);

        public static IEnumerable<RoomType> EnumerateRoomTypes() =>
            from x in Enumerable.Range(1, 15) select (RoomType)x;

        public static List<Direction> ValidDirections(this Point p, int width, int height) =>
            (from d in Directions where p.Add(d).IsInRange(width, height) select d).ToList();

        public static IEnumerable<Point> Neighbors(this Point p, int width, int height) =>
            from offset in Offsets let p2 = p + offset where p2.IsInRange(width, height) select p2;

        public static bool IsInRange(this Point p, int width, int height) => p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height;

        public static T At<T>(this T[,] grid, Point p) => grid[p.X, p.Y];

        public static Point Add(this Point p, Direction dir) => dir == Direction.None ? Invalid : p + Offsets[(int)(dir - 1)];

        public static Direction Flip(this Direction dir)
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

        public static RoomType ToRoomType(this Direction dir)
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
                    return RoomType.Mystery;
            }
        }

        public static bool IsLeaf(this RoomType roomType)
        {
            var type = roomType & (RoomType.W | RoomType.E | RoomType.S | RoomType.N);
            return type == RoomType.W || roomType == RoomType.E || roomType == RoomType.S || roomType == RoomType.N;
        }

        public static bool IsOpened(this RoomType t) => t > 0 && (t & RoomType.Mystery) == 0;

        public static bool IsBase(this RoomType roomType) => (roomType & RoomType.Base) != 0;

        public static bool IsBoss(this RoomType roomType) => (roomType & RoomType.Boss) != 0;

        public static string ToResourceString(this RoomType type)
        {
            if (type <= 0 || type == RoomType.Mystery)
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

        public static T Choice<T>(this Random random, IList<T> collection) => collection[random.Next(collection.Count)];

        // Randomly shuffles a list
        public static void Shuffle<T>(this Random random, IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Direction NextDirection(this Random random) => random.Choice(Directions);

        public static Point NextPoint(this Random random, int width, int height) => new Point(random.Next(width), random.Next(height));

        public static string ToShortString(this Point p) => p == Invalid ? "-" : $"({p.X},{p.Y})";
        public static string ToChessString(this Point p) => p == Invalid ? "-" : $"{(char)(p.X + 'a')}{(char)(p.Y + '1')}";

        // Parse chess notation
        public static Point ParseChess(string text)
        {
            if (text.Length >= 2)
                return new Point(text[0] - 'a', text[1] - '1');
            return Invalid;
        }

        public static int RandomRoomcount(this Random random, FloorSize floorSize)
        {
            if (random.Next(3) == 0)
            {
                // Generate 50-53 or 61-64
                return 50 + random.Next(4) + 11 * random.Next(2);
            }
            else
            {
                return 54 + random.Next(7);
            }
        }
    }
}
