using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MapGenerator
{
    /// <summary>
    /// Represents a (completed) dungeoneering map.
    /// </summary>
    public class Map
    {
        // ParentDirs[x,y] points to the parent square of (x,y).
        private Direction[,] parentDirs;

        public Map(Direction[,] parentDirs)
        {
            this.parentDirs = parentDirs;
        }

        public Map(int width, int height)
        {
            parentDirs = new Direction[width, height];
        }

        public Direction this[Point p]
        {
            get => parentDirs.At(p);
            set { parentDirs[p.X, p.Y] = value; }
        }

        public int Width => parentDirs.GetLength(0);
        public int Height => parentDirs.GetLength(1);
        public int MaxRooms => Width * Height;
        public Point Base { get; set; } = MapUtils.Invalid;
        public Point Boss { get; set; } = MapUtils.Invalid;
        public HashSet<Point> CritRooms { get; set; } = new HashSet<Point>();

        private readonly Brush critBrush = new SolidBrush(Color.FromArgb(128, 128, 255, 255));

        public Point Parent(Point p) => p.Add(this[p]);

        // Return the directions of the children.
        public IEnumerable<Direction> ChildrenDirs(Point p)
        {
            return from dir in MapUtils.Directions
                   let p2 = p.Add(dir)
                   where p2.IsInRange(Width, Height) && this[p2] == dir.Flip()
                   select dir;
        }

        // If no neighbors, gap.
        public RoomType GetRoomType(Point p)
        {
            var roomType = this[p].ToRoomType();
            if (p != Base && roomType <= 0)
                return roomType;

            foreach (var dir in ChildrenDirs(p))
                roomType |= dir.ToRoomType();

            return roomType == 0 ? RoomType.Gap : roomType;
        }

        public bool IsDeadEnd(Point p, bool turningRequired = false)
        {
            // Base, boss, and gaps are not dead ends
            if (p == Base || p == Boss || !IsRoom(p))
                return false;
            // Check if anything has it as a parent.
            if (ChildrenDirs(p).Count() > 0)
                return false;
            if (turningRequired)
            {
                var d = this[p];
                var p2 = p.Add(d);
                if (this[p2] == d)
                    return false;
            }
            return true;
        }

        public bool IsBonusDeadEnd(Point p)
        {
            return IsDeadEnd(p) && !CritRooms.Contains(p);
        }

        // aka non-gap
        public bool IsRoom(Point p)
        {
            return p == Base || (p.IsInRange(Width, Height) && this[p] != Direction.None);
        }

        public void AddCritRoom(Point p)
        {
            while (p != Base)
            {
                CritRooms.Add(p);
                p = Parent(p);
            }
        }

        public List<Point> GetDeadEnds(bool turningOnly = false)
        {
            return (from p in MapUtils.GridPoints(Width, Height)
                    where IsDeadEnd(p, turningOnly)
                    select p).ToList();
        }

        public List<Point> GetBonusDeadEnds()
        {
            return (from p in MapUtils.GridPoints(Width, Height)
                    where IsBonusDeadEnd(p)
                    select p).ToList();
        }

        public int SubtreeSize(Point p)
        {
            int count = 0;

            void Visit(Point p2)
            {
                ++count;

                foreach (var d in ChildrenDirs(p2))
                    Visit(p2.Add(d));
            }

            Visit(p);
            return count;
        }

        // Gets the number of neighboring gaps
        public int GetDensity(Point p)
        {
            return (from d in MapUtils.Directions
                    let p2 = p.Add(d)
                    where p2.IsInRange(Width, Height) && IsRoom(p2)
                    select p2).Count();
        }

        // Returns a list of non-gap rooms.
        public List<Point> GetRooms()
        {
            return (from p in MapUtils.GridPoints(Width, Height)
                    where IsRoom(p)
                    select p).ToList();
        }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    parentDirs[x, y] = Direction.None;
                }
            }

            Base = MapUtils.Invalid;
            Boss = MapUtils.Invalid;
            CritRooms.Clear();
        }

        // Precondition: p must be a dead end.
        public void RemoveDeadEnd(Point p)
        {
            parentDirs[p.X, p.Y] = Direction.None;
        }

        public Bitmap ToImage(bool drawCritRooms = false, bool drawDeadEnds = true)
        {
            var bmp = new Bitmap(32 * Width, 32 * Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var p = new Point(x, y);
                        var roomBmp = RoomTypeToBitmap(GetRoomType(p));
                        g.DrawImage(roomBmp, x * 32, (7 - y) * 32, 32, 32);
                        if (drawDeadEnds && IsDeadEnd(p))
                        {
                            g.FillRectangle(Brushes.Red, x * 32 + 14, (7 - y) * 32 + 14, 4, 4);
                        }
                        if (drawCritRooms && CritRooms.Contains(p))
                        {
                            g.FillRectangle(critBrush, x * 32 + 14, (7 - y) * 32 + 14, 4, 4);
                        }
                    }
                }

                g.DrawImage(Resources.BaseOverlay, 32 * Base.X, 32 * (7 - Base.Y));
                g.DrawImage(Resources.BossOverlay, 32 * Boss.X, 32 * (7 - Boss.Y));
            }
            return bmp;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int y = Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    var a = parentDirs[x, y];
                    sb.Append(a == Direction.None ? "-" : a.ToString());
                }
                if (y > 0)
                    sb.AppendLine();
            }
            return sb.ToString();
        }

        static Bitmap RoomTypeToBitmap(RoomType type) => (Bitmap)Resources.ResourceManager.GetObject(type.ToResourceString());
    }
}
