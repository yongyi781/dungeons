using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Dungeons.Common
{
    /// <summary>
    /// Represents a (completed) dungeoneering map.
    /// </summary>
    public class Map
    {
        // ParentDirs[x,y] points to the parent square of (x,y).
        private readonly Direction[,] parentDirs;

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
        public SortedSet<Point> CritEndpoints { get; } = new SortedSet<Point>(new PointComparer());

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

            return roomType == 0 ? RoomType.None : roomType;
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
            return IsDeadEnd(p) && !CritEndpoints.Contains(p);
        }

        // aka non-gap
        public bool IsRoom(Point p)
        {
            return p == Base || (p.IsInRange(Width, Height) && this[p] != Direction.None);
        }

        public void AddCritEndpoint(Point p)
        {
            if (p != Base)
                CritEndpoints.Add(p);
        }

        // Precondition: p is in CritEndpoints.
        public void BacktrackCritEndpoint(Point p)
        {
            if (CritEndpoints.Remove(p))
            {
                AddCritEndpoint(Parent(p));
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

        public int DistanceToBase(Point p)
        {
            int dist = 0;
            TraverseToBase(p, _ => ++dist);
            return dist;
        }

        public void TraverseToBase(Point p, Action<Point> callback)
        {
            // Prevent infinite loops
            for (int i = 0; p != Base && i < Width * Height; i++, p = Parent(p))
            {
                callback(p);
            }
        }

        public void TraverseSubtree(Point p, Action<Point> callback)
        {
            void Visit(Point p2)
            {
                callback(p2);
                foreach (var d in ChildrenDirs(p2))
                    Visit(p2.Add(d));
            }

            Visit(p);
        }

        public int SubtreeSize(Point p)
        {
            if (!p.IsInRange(Width, Height))
                return 0;

            int count = 0;
            TraverseSubtree(p, _ => ++count);
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
            CritEndpoints.Clear();
        }

        public SortedSet<Point> GetCritRooms()
        {
            var set = new SortedSet<Point>(new PointComparer());
            set.Add(Base);
            foreach (var e in CritEndpoints)
            {
                TraverseToBase(e, p => set.Add(p));
            }
            return set;
        }

        // Precondition: p must be a dead end.
        public void RemoveDeadEnd(Point p)
        {
            parentDirs[p.X, p.Y] = Direction.None;
        }

        public string ToPrettyString()
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
            }
            var critStr = string.Join(",", from x in CritEndpoints where x != Boss select x.ToChessString());
            return $"{Width},{Height},{sb},{Base.ToChessString()},{Boss.ToChessString()},{critStr}";
        }
    }
}
