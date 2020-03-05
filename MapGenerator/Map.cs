using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MapGenerator
{
    public class Map
    {
        public Map(Direction[,] parents)
        {
            Parents = parents;
        }

        public Direction[,] Parents { get; set; }
        public int Width => Parents.GetLength(1);
        public int Height => Parents.GetLength(0);
        public Point Base { get; set; } = MapUtils.Invalid;
        public Point Boss { get; set; } = MapUtils.Invalid;
        public HashSet<Point> CritRooms { get; set; } = new HashSet<Point>();

        private Brush critBrush = new SolidBrush(Color.FromArgb(128, 128, 255, 255));

        public bool IsDeadEnd(Point p, bool turningRequired = false)
        {
            // Base, boss, and gaps are not dead ends
            if (p == Base || p == Boss || Parents[p.X, p.Y] == Direction.None)
                return false;
            // Check if anything has it as a parent.
            for (int i = 0; i < 4; i++)
            {
                var p2 = p + MapUtils.Offsets[i];
                if (MapUtils.IsInRange(p2) && Parents[p2.X, p2.Y] == MapUtils.Flip(MapUtils.Directions[i]))
                    return false;
            }
            if (turningRequired)
            {
                var d = Parents[p.X, p.Y];
                var p2 = p.Add(d);
                if (Parents[p2.X, p2.Y] == d)
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
            return Parents[p.X, p.Y] != Direction.None;
        }

        public void AddCritRoom(Point p)
        {
            while (p != Base)
            {
                CritRooms.Add(p);
                p = p.Add(Parents[p.X, p.Y]);
            }
        }

        // Returns the neighbors of a point.
        public List<Point> GetNeighbors(Point p)
        {
            var neighbors = new List<Point>();
            // Check if anything has it as a parent.
            for (int i = 0; i < 4; i++)
            {
                var p2 = p + MapUtils.Offsets[i];
                if (MapUtils.IsInRange(p2) && Parents[p2.X, p2.Y] == MapUtils.Flip(MapUtils.Directions[i]))
                    neighbors.Add(p2);
            }
            return neighbors;
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

        // Gets the number of neighboring gaps
        public int GetDensity(Point p)
        {
            return (from d in MapUtils.Directions
                    let p2 = p.Add(d)
                    where MapUtils.IsInRange(p2) && IsRoom(p2)
                    select p2).Count();
        }

        // Returns a list of non-gap rooms.
        public List<Point> GetRooms()
        {
            return (from p in MapUtils.GridPoints(Width, Height)
                    where IsRoom(p)
                    select p).ToList();
        }

        // Only properly removes dead ends.
        public void RemoveRoom(Point p)
        {
            Parents[p.X, p.Y] = Direction.None;
        }

        // If no neighbors, gap.
        public RoomType GetRoomType(Point p)
        {
            RoomType[] types = { RoomType.W, RoomType.E, RoomType.S, RoomType.N };
            var roomType = MapUtils.ToRoomType(Parents[p.X, p.Y]);
            for (int i = 0; i < MapUtils.Offsets.Length; i++)
            {
                var p2 = p + MapUtils.Offsets[i];
                if (MapUtils.IsInRange(p2) && Parents[p2.X, p2.Y] == MapUtils.Flip(MapUtils.Directions[i]))
                    roomType |= types[i];
            }
            return roomType == RoomType.NotOpened ? RoomType.Gap : roomType;
        }

        public Bitmap ToImage(bool drawDeadEnds = true, bool drawCritRooms = false)
        {
            var bmp = new Bitmap(32 * 8, 32 * 8);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        var p = new Point(x, y);
                        var roomBmp = MapUtils.GetRoomTypeBitmap(GetRoomType(p));
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
            return Parents.ToPrettyString();
        }
    }
}
