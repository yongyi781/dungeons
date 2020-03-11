using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Point Base { get; set; } = MapUtils.Invalid;
        public Point Boss { get; set; } = MapUtils.Invalid;
        public Direction BossFaceDirection { get; set; }
        public SortedSet<Point> CritEndpoints { get; } = new SortedSet<Point>(new PointComparer());

        public int Width => parentDirs.GetLength(0);
        public int Height => parentDirs.GetLength(1);
        public Size Size => new Size(Width, Height);
        public int MaxRooms => Width * Height;
        public int Roomcount => GetRooms().Count();
        public int GapCount => GetGaps().Count();

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

        public bool IsDeadEnd(Point p, bool includeBaseAndBoss = false)
        {
            // Gap is not a dead end, don't count boss unless inlcudeBaseAndBoss is true.
            if (!IsRoom(p) || (p == Boss && !includeBaseAndBoss))
                return false;

            if (p == Base && includeBaseAndBoss)
            {
                // Base is a dead end if it has exactly one child.
                return ChildrenDirs(Base).Count() == 1;
            }

            // Check if anything has it as a parent.
            return ChildrenDirs(p).Count() == 0;
        }

        public bool IsBonusDeadEnd(Point p)
        {
            return IsDeadEnd(p) && !CritEndpoints.Contains(p);
        }

        // aka non-gap
        public bool IsRoom(Point p)
        {
            return p == Base || (p.IsInRange(Width, Height) && this[p] > Direction.None);
        }

        public void AddCritEndpoint(Point point)
        {
            CritEndpoints.Add(point);
        }

        // Trim crit endpoints.
        public void SimplifyCrit()
        {
            foreach (var point in CritEndpoints.ToList())
            {
                if (!CritEndpoints.Contains(point))
                    continue;
                if (point == Base || point == Boss)
                    CritEndpoints.Remove(point);
                TraverseToBase(point, p =>
                {
                    if (p != point)
                        CritEndpoints.Remove(p);
                });
            }
        }

        // Precondition: point is in CritEndpoints.
        public void BacktrackCritEndpoint(Point p)
        {
            var parent = Parent(p);
            if (CritEndpoints.Remove(p) && parent != Base && parent != MapUtils.Invalid)
            {
                AddCritEndpoint(parent);
            }
        }

        public List<Point> GetDeadEnds(bool includeBaseAndBoss = false)
        {
            return (from p in MapUtils.Range2D(Width, Height)
                    where IsDeadEnd(p, includeBaseAndBoss)
                    select p).ToList();
        }

        public List<Point> GetBonusDeadEnds()
        {
            return (from p in MapUtils.Range2D(Width, Height)
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
            for (int i = 0; p != MapUtils.Invalid && p != Base && i < Width * Height; i++, p = Parent(p))
                callback(p);
        }

        /// <summary>
        /// Traverses the map from a specified point all the way to base, excluding base.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <param name="callback">The function to perform on each point. The return value of callback determines whether to stop early.</param>
        /// <returns>true if callback returned true at least once; otherwise, false.</returns>
        public bool TraverseToBase(Point point, Func<Point, bool> callback)
        {
            // Prevent infinite loops
            for (int i = 0; point != MapUtils.Invalid && point != Base && i < Width * Height; i++, point = Parent(point))
                if (callback(point))
                    return true;
            return false;
        }

        public void TraverseSubtree(Point p, Action<Point, int> callback)
        {
            void Visit(Point p2, int depth)
            {
                if (depth > MaxRooms)
                {
                    Debug.WriteLine("Something went horribly wrong...");
                    return;
                }

                callback(p2, depth);
                foreach (var d in ChildrenDirs(p2))
                    Visit(p2.Add(d), depth + 1);
            }

            Visit(p, 0);
        }

        // Traverses the entire map using the specified root.
        // callback takes parameters point, direction to previous point, and depth.
        public void TraverseWholeTree(Point root, Action<Point, Direction, int> callback)
        {
            if (!root.IsInRange(Width, Height))
                return;

            HashSet<Point> visited = new HashSet<Point>();

            void Visit(Point p, int dist)
            {
                visited.Add(p);

                // Traverse both parent and children, if not visited before.
                if (this[p] != Direction.None)
                {
                    var parent = Parent(p);
                    if (!visited.Contains(parent))
                    {
                        Visit(parent, dist + 1);
                        callback(parent, this[p].Flip(), dist + 1);
                    }
                }
                foreach (var d in ChildrenDirs(p))
                {
                    if (!visited.Contains(p.Add(d)))
                    {
                        Visit(p.Add(d), dist + 1);
                        callback(p.Add(d), d.Flip(), dist + 1);
                    }
                }
            }

            Visit(root, 0);
        }

        public int SubtreeSize(Point point)
        {
            if (!point.IsInRange(Width, Height))
                return 0;

            int count = 0;
            TraverseSubtree(point, (_, _2) => ++count);
            return count;
        }

        public Dictionary<Direction, int> SubtreeSizes(Point point)
        {
            return (from dir in ChildrenDirs(point) select (dir, SubtreeSize(point.Add(dir)))).ToDictionary(k => k.dir, k => k.Item2);
        }

        // Gets the number of neighboring gaps
        public int GetDensity(Point point)
        {
            return (from d in MapUtils.Directions
                    let p = point.Add(d)
                    where p.IsInRange(Width, Height) && IsRoom(p)
                    select p).Count();
        }

        // Returns a list of non-gap rooms.
        public List<Point> GetRooms()
        {
            return (from p in MapUtils.Range2D(Width, Height)
                    where IsRoom(p)
                    select p).ToList();
        }

        // Returns a list of specifically gaps.
        public List<Point> GetGaps()
        {
            return (from p in MapUtils.Range2D(Width, Height)
                    where this[p] == Direction.Gap
                    select p).ToList();
        }

        // Returns a list of specifically gaps.
        public List<Point> GetNonGaps()
        {
            return (from p in MapUtils.Range2D(Width, Height)
                    where this[p] != Direction.Gap
                    select p).ToList();
        }

        public List<Point> GetNonRooms()
        {
            return (from p in MapUtils.Range2D(Width, Height)
                    where !IsRoom(p)
                    select p).ToList();
        }

        // Returns the height of the tree underlying the map.
        public int GetTreeHeight()
        {
            var maxDepth = -1;

            TraverseSubtree(Base, (_, d) => maxDepth = Math.Max(maxDepth, d));

            return maxDepth;
        }

        /// <summary>
        /// Farthest point from point.
        /// </summary>
        public (Point, int) GetFarthestPoint(Point point)
        {
            var maxDist = 0;
            var farthest = point;

            TraverseWholeTree(point, (p, _, dist) =>
            {
                if (maxDist < dist)
                {
                    maxDist = dist;
                    farthest = p;
                }
            });

            return (farthest, maxDist);
        }

        /// <summary>
        /// Max distance from point to another point: https://en.wikipedia.org/wiki/Distance_(graph_theory).
        /// </summary>
        public int GetEccentricity(Point point) => GetFarthestPoint(point).Item2;

        public int GetDiameter() => GetEccentricity(GetFarthestPoint(Base).Item1);

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    parentDirs[x, y] = Direction.None;
                }
            }

            CritEndpoints.Clear();
        }

        public HashSet<Point> GetCritRooms()
        {
            var set = new HashSet<Point> { Base };
            var critWithBoss = CritEndpoints.Union(new Point[] { Boss }).ToList();
            foreach (var e in critWithBoss)
            {
                TraverseToBase(e, p => { set.Add(p); });
            }
            return set;
        }

        public bool IsCrit(Point point)
        {
            foreach (var e in CritEndpoints.Union(new Point[] { Boss }))
                if (TraverseToBase(e, p => p == point))
                    return true;
            return false;
        }

        // Precondition: point must be a dead end.
        public void RemoveDeadEnd(Point point)
        {
            if (point == Base)
            {
                // Better have just one child. Replace base with its child.
                var dir = ChildrenDirs(Base).SingleOrDefault();
                if (dir != Direction.None)
                {
                    Base = Base.Add(dir);
                    this[Base] = Direction.None;
                    // Remove new base from crit endpoints
                    CritEndpoints.Remove(Base);
                }
            }
            else if (CritEndpoints.Contains(point))
            {
                CritEndpoints.Remove(point);
                AddCritEndpoint(Parent(point));
            }
            parentDirs[point.X, point.Y] = Direction.None;
        }

        // Sets the new base, updating parents to point to the new base.
        public void Rebase(Point newBase)
        {
            if (newBase == Base)
                return;
            if (!IsRoom(newBase))
                throw new ArgumentException("New base is not a room.", nameof(newBase));

            TraverseWholeTree(newBase, (p, dir, _) => this[p] = dir);
            this[newBase] = Direction.None;
            Base = newBase;
        }

        public string GetMapString()
        {
            return parentDirs.ToPrettyString(a => DirectionToChar(a), string.Empty);
        }

        public override string ToString()
        {
            var sep = " ";
            var mapStr = parentDirs.ToPrettyString(a => DirectionToChar(a), string.Empty, "/");

            var critStr = string.Join(sep, from x in CritEndpoints where x != Boss select x.ToChessString());
            return $"{Width}{sep}{Height}{sep}{mapStr}{sep}{Base.ToChessString()}{sep}{Boss.ToChessString()}{sep}{critStr}";
        }

        private string DirectionToChar(Direction dir)
        {
            return dir == Direction.Gap ? "-" : dir == Direction.None ? "." : dir.ToString();
        }
    }
}
