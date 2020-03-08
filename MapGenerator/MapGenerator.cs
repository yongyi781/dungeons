using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace MapGenerator
{
    public class MapGenerator
    {
        private readonly Random random;

        public MapGenerator(int seed, FloorSize floorSize)
        {
            random = new Random(seed);
            FloorSize = floorSize;
        }

        public FloorSize FloorSize { get; }

        public void GenerateBaseLocation(Map map)
        {
            map.Base = random.NextPoint(map.Width, map.Width);
        }

        // Generates a completely random map with possible loops.
        public async Task GenerateWithPossibleLoops(Map map, Func<Task> callback)
        {
            foreach (var p in MapUtils.Range2D(map.Width, map.Height))
            {
                if (p != map.Base)
                {
                    map[p] = random.Choice(p.ValidDirections(map.Width, map.Height));

                    if (callback != null)
                        await callback.Invoke();
                }
            }
        }

        // Choose from open edges rather than rooms.
        public void GeneratePrim(Map map, bool oneWayRoot = false)
        {
            var openEnds = new List<(Point, Direction)>();
            foreach (var room in map.GetRooms())
            {
                AddNeighborsToFrontier(room);
            }

            while (openEnds.Count > 0)
            {
                // Select random element in openEnds and remove every other entry with the same point.
                var (p, dir) = random.Choice(openEnds);
                openEnds.RemoveAll(a => a.Item1 == p);
                map[p] = dir;
                AddNeighborsToFrontier(p);
            }

            void AddNeighborsToFrontier(Point p)
            {
                foreach (var dir in MapUtils.Directions)
                {
                    var p2 = p.Add(dir);
                    if (p2.IsInRange(map.Width, map.Height) && !map.IsRoom(p2))
                        openEnds.Add((p2, dir.Flip()));
                }
            }
        }

        // Choose from open edges rather than rooms.
        public void GeneratePrimDeadEndBase(Map map, Direction baseFaceDir)
        {
            var openEnds = new List<(Point, Direction)> { (map.Base.Add(baseFaceDir), baseFaceDir.Flip()) };

            while (openEnds.Count > 0)
            {
                // Select random element in openEnds and remove every other entry with the same point.
                var (p, dir) = random.Choice(openEnds);
                openEnds.RemoveAll(a => a.Item1 == p);
                map[p] = dir;
                AddNeighborsToFrontier(p);
            }

            void AddNeighborsToFrontier(Point p)
            {
                foreach (var dir in MapUtils.Directions)
                {
                    var p2 = p.Add(dir);
                    if (p2.IsInRange(map.Width, map.Height) && map[p2] != Direction.Gap && !map.IsRoom(p2))
                        openEnds.Add((p2, dir.Flip()));
                }
            }
        }

        //// Generate entire map, starting with boss, also assigning crit. This also clears the map.
        //public void GeneratePrimBackwards(Map map)
        //{
        //    map.Clear();
        //    map.Boss = random.NextPoint(map.Width, map.Height);

        //    var bossFaceDir = random.Choice(map.Boss.ValidDirections(map.Width, map.Height));
        //    var openEnds = new List<(Point, Direction)> { (map.Boss.Add(bossFaceDir), bossFaceDir.Flip()) };

        //    while (openEnds.Count > 0)
        //    {
        //        // Select random element in openEnds and remove every other entry with the same point.
        //        var (p, dir) = random.Choice(openEnds);
        //        openEnds.RemoveAll(a => a.Item1 == p);
        //        map[p] = dir;
        //        AddNeighborsToFrontier(p);
        //    }

        //    void AddNeighborsToFrontier(Point p)
        //    {
        //        foreach (var dir in MapUtils.Directions)
        //        {
        //            var p2 = p.Add(dir);
        //            if (p2.IsInRange(map.Width, map.Height) && p2 != map.Boss && !map.IsRoom(p2))
        //                openEnds.Add((p2, dir.Flip()));
        //        }
        //    }
        //}

        public void GeneratePrimDoorVariant(Map map)
        {
            var frontier = new List<Point>();
            foreach (var room in map.GetRooms())
            {
                AddNeighborsToFrontier(room);
            }
            while (frontier.Count > 0)
            {
                // Select random point in frontier and remove it
                var p = random.Choice(frontier);
                frontier.Remove(p);
                var validDirs = (from d in MapUtils.Directions
                                 let p2 = p.Add(d)
                                 where map.IsRoom(p2)
                                 select d).ToArray();
                map[p] = random.Choice(validDirs);
                AddNeighborsToFrontier(p);
            }

            void AddNeighborsToFrontier(Point p)
            {
                foreach (var dir in MapUtils.Directions)
                {
                    var p2 = p.Add(dir);
                    if (p2.IsInRange(map.Width, map.Height) && !map.IsRoom(p2) && !frontier.Contains(p2))
                        frontier.Add(p2);
                }
            }
        }

        // This makes a uniform distribution of spanning trees.
        public void GenerateAldousBroder(Map map, Direction baseFaceDir)
        {
            // Random walk until map is full
            var visited = new HashSet<Point>(map.GetRooms());
            var p = map.Base.Add(baseFaceDir);
            visited.Add(p);
            map[p] = baseFaceDir.Flip();
            while (visited.Count < map.MaxRooms - map.GapCount)
            {
                var validDirs = (from d in p.ValidDirections(map.Width, map.Height)
                                 let p2 = p.Add(d)
                                 where p2 != map.Base && map[p2] != Direction.Gap
                                 select d).ToList();
                var dir = random.Choice(validDirs);
                p = p.Add(dir);
                if (!visited.Contains(p))
                {
                    visited.Add(p);
                    map[p] = dir.Flip();
                }
            }
        }

        public void AssignCritRooms(Map map)
        {
            var desiredCritCount = random.Next(FloorSize.MinCrit, FloorSize.MaxCrit + 1);
            Debug.WriteLine("Desired crit: " + desiredCritCount);

            // Boss is crit.
            map.AddCritEndpoint(map.Boss);
            
            // Assign some random (not-too-far) dead ends to crit until crit count exceeds desired.
            var deadEnds = (from de in map.GetDeadEnds() where map.DistanceToBase(de) < desiredCritCount select de).ToList();
            random.Shuffle(deadEnds);
            for (int i = 0; map.GetCritRooms().Count < desiredCritCount && i < deadEnds.Count; i++)
                map.AddCritEndpoint(deadEnds[i]);

            // Trim until crit count equals desired.
            while (map.GetCritRooms().Count > desiredCritCount)
            {
                var nonBoss = (from e in map.CritEndpoints where e != map.Boss select e).ToList();
                map.BacktrackCritEndpoint(random.Choice(nonBoss));
            }
        }

        // Select dead ends at random
        public void MakeGaps(Map map, int roomcount = 0)
        {
            if (roomcount == 0)
                roomcount = random.RandomRoomcount(FloorSize);
            for (int i = 0; i < map.MaxRooms - roomcount; i++)
                RemoveBonusDeadEnd(map);
        }

        // Returns the boss face direction.
        public Direction MakeBossAndStartingGaps(Map map, int roomcount = 0)
        {
            if (roomcount == 0)
                roomcount = random.RandomRoomcount(FloorSize);

            Direction bossFaceDir = Direction.None;
            var solvable = false;

            while (!solvable)
            {
                map.Clear();

                var points = MapUtils.Range2D(map.Width, map.Height).ToList();
                random.Shuffle(points);
                map.Boss = points[0];
                for (int i = 0; i < map.MaxRooms - roomcount; i++)
                {
                    map[points[i + 1]] = Direction.Gap;
                }
                var validDirs = (from d in map.Boss.ValidDirections(map.Width, map.Height)
                                 where map[map.Boss.Add(d)] != Direction.Gap
                                 select d).ToList();
                if (validDirs.Count == 0)
                    continue;
                bossFaceDir = random.Choice(validDirs);
                solvable = IsSolvable();
            }

            return bossFaceDir;

            bool IsSolvable()
            {
                var visited = new HashSet<Point> { map.Boss };
                var visitCount = 1;

                void Visit(Point p)
                {
                    visited.Add(p);
                    ++visitCount;
                    foreach (var dir in p.ValidDirections(map.Width, map.Height))
                    {
                        var p2 = p.Add(dir);
                        if (map[p2] != Direction.Gap && !visited.Contains(p2))
                            Visit(p2);
                    }
                }

                Visit(map.Boss.Add(bossFaceDir));
                return visitCount == roomcount;
            }
        }

        public void RemoveBonusDeadEnd(Map map)
        {
            // Remove a random turning dead end if possible, otherwise remove any dead end.
            var deadEnds = map.GetBonusDeadEnds();
            if (deadEnds.Count > 0)
                map.RemoveDeadEnd(random.Choice(deadEnds));
        }

        public void Rebase(Map map)
        {
            var rooms = (from r in map.GetCritRooms()
                         where r != map.Boss
                         select r).ToList();
            if (rooms.Count > 0)
                map.Rebase(random.Choice(rooms));
        }
    }
}
