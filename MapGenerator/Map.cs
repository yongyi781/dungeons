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

        public bool IsDeadEnd(Point p)
        {
            if (p == Base || p == Boss || Parents[p.X, p.Y] == Direction.None)
                return false;
            for (int i = 0; i < 4; i++)
            {
                var p2 = p + MapUtils.Offsets[i];
                if (MapUtils.IsInRange(p2) && Parents[p2.X, p2.Y] == MapUtils.Flip(MapUtils.Directions[i]))
                    return false;
            }
            return true;
        }

        // aka non-gap
        public bool IsRoom(Point p)
        {
            return Parents[p.X, p.Y] != Direction.None;
        }

        public List<Point> GetDeadEnds()
        {
            return (from y in Enumerable.Range(0, Height) from x in Enumerable.Range(0, Width)
                    let p = new Point(x, y)
                    where IsDeadEnd(p)
                    select p).ToList();
        }

        // Gets the number of neighboring gaps
        public int CountGapNeighbors(Point p)
        {
            return (from y in Enumerable.Range(-1, 1)
                    from x in Enumerable.Range(-1, 1)
                    where (x != 0 || y != 0)
                    let p2 = new Point(p.X + x, p.Y + y)
                    where MapUtils.IsInRange(p2) && !IsRoom(p2)
                    select p2).Count();
        }

        public List<Point> GetRooms()
        {
            return (from y in Enumerable.Range(0, Height)
                    from x in Enumerable.Range(0, Width)
                    let p = new Point(x, y)
                    where IsRoom(p)
                    select p).ToList();
        }

        public void RemoveDeadEnd(Point p)
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

        public Bitmap ToImage(bool drawDeadEnds = false)
        {
            var bmp = new Bitmap(32 * 8, 32 * 8);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        var roomBmp = MapUtils.GetRoomTypeBitmap(GetRoomType(new Point(x, y)));
                        g.DrawImage(roomBmp, x * 32, (7 - y) * 32, 32, 32);
                        if (drawDeadEnds && IsDeadEnd(new Point(x, y)))
                        {
                            g.FillRectangle(Brushes.Red, x * 32 + 14, (7 - y) * 32 + 14, 4, 4);
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
