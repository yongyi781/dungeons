﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Dungeons.Common
{
    public static class MapUtils
    {
        // Number of pixels per room.
        public const int RoomSize = 32;

        public static readonly Point Invalid = new Point(-1, -1);

        public static readonly Size[] Offsets = { new Size(-1, 0), new Size(1, 0), new Size(0, -1), new Size(0, 1) };
        public static readonly Direction[] Directions = { Direction.W, Direction.E, Direction.S, Direction.N };

        // [0..8) x [0..8)
        public static IEnumerable<Point> Range2D(int width, int height) =>
            from y in Enumerable.Range(0, height) from x in Enumerable.Range(0, width) select new Point(x, y);

        public static IEnumerable<RoomType> EnumerateRoomTypes() =>
            from x in Enumerable.Range(1, 15) select (RoomType)x;

        public static List<Direction> ValidDirections(this Point p, int width, int height) =>
            (from d in Directions where p.Add(d).IsInRange(width, height) select d).ToList();

        public static IEnumerable<Point> Neighbors(this Point p, int width, int height) =>
            from offset in Offsets let p2 = p + offset where p2.IsInRange(width, height) select p2;

        public static bool IsInRange(this Point p, int width, int height) => p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height;

        public static T At<T>(this T[,] grid, Point p) => grid[p.X, p.Y];

        public static Point Add(this Point p, Direction dir) => dir switch
        {
            Direction.W => new Point(p.X - 1, p.Y),
            Direction.E => new Point(p.X + 1, p.Y),
            Direction.S => new Point(p.X, p.Y - 1),
            Direction.N => new Point(p.X, p.Y + 1),
            _ => Invalid,
        };

        public static bool IsOnWall(this Point p, int width, int height) => p.X == 0 || p.X == width - 1 || p.Y == 0 || p.Y == height - 1;

        public static Direction Flip(this Direction dir) => dir switch
        {
            Direction.W => Direction.E,
            Direction.E => Direction.W,
            Direction.S => Direction.N,
            Direction.N => Direction.S,
            _ => Direction.None,
        };

        public static RoomType ToRoomType(this Direction dir) => dir switch
        {
            Direction.W => RoomType.W,
            Direction.E => RoomType.E,
            Direction.S => RoomType.S,
            Direction.N => RoomType.N,
            _ => RoomType.Gap,
        };

        public static bool IsLeaf(this RoomType roomType)
        {
            var t = roomType & (RoomType.W | RoomType.E | RoomType.S | RoomType.N);
            return t == RoomType.W || t == RoomType.E || t == RoomType.S || t == RoomType.N;
        }

        public static bool IsOpened(this RoomType t) => t > 0 && (t & RoomType.Mystery) == 0;
        public static bool IsBase(this RoomType roomType) => (roomType & RoomType.Base) != 0;
        public static bool IsBoss(this RoomType roomType) => (roomType & RoomType.Boss) != 0;
        public static bool IsCrit(this RoomType roomType) => (roomType & RoomType.Crit) != 0;
        public static bool IsMystery(this RoomType roomType) => (roomType & RoomType.Mystery) != 0;

        public static string ToResourceString(this RoomType type)
        {
            if (type <= 0)
                return "NotOpened";

            var str = "Room";
            if (type.IsCrit())
                str = "Crit";
            else if (type.IsMystery())
                str = "Mystery";

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

        // Prints chess string if p.X < 26, otherwise the short string.
        public static string ToChessString(this Point p) => p == Invalid ? "-" :
            p.X < 26 ? $"{(char)(p.X + 'a')}{p.Y + 1}" : p.ToShortString();

        public static double[,] Normalize(this int[,] matrix)
        {
            var result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            int total = 0;
            foreach (var x in matrix)
                total += x;
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    result[x, y] = (double)matrix[x, y] / total;
                }
            }
            return result;
        }

        // Prints matrix where (0,0) is bottom left.
        public static string ToPrettyString<T>(this T[,] matrix, Func<T, string> converter, string separator = ",", string lineSeparator = "\r\n")
        {
            var sb = new StringBuilder();
            for (int y = matrix.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    sb.Append(converter(matrix[x, y]));
                    if (x < matrix.GetLength(0) - 1)
                        sb.Append(separator);
                }
                if (y > 0)
                    sb.Append(lineSeparator);
            }
            return sb.ToString();
        }

        public static string ToPrettyString(this IEnumerable<Point> points) => "[" + string.Join(",", from p in points select p.ToChessString()) + "]";

        // Parse chess notation
        public static Point ParseChess(string text)
        {
            if (text.Length >= 2)
                return new Point(text[0] - 'a', text[1] - '1');
            return Invalid;
        }

        public static int RandomRoomcount(this Random random, FloorSize floorSize) =>
            random.Next(3) == 0 ? random.Next(floorSize.MinRC, floorSize.MaxRC) : random.Next(floorSize.MinRC + floorSize.RareRCSpread, floorSize.MaxRC - floorSize.RareRCSpread);

        // Color utils
        public static bool IsBetween(this Color color, Color min, Color max) =>
            color.R >= min.R && color.R <= max.R &&
                color.G >= min.G && color.G <= max.G &&
                color.B >= min.B && color.B <= max.B;
    }
}
