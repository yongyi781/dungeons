using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Replaces the given map with generated map using the specified algorithm.
        /// </summary>
        /// <param name="map">The map to generate onto.</param>
        /// <param name="roomcount">The roomcount. If it is zero, use a random roomcount.</param>
        /// <param name="spanningTreeAlgorithm">The spanning tree algorithm to use.</param>
        public void Generate(Map map, int roomcount = 0, string spanningTreeAlgorithm = "Prim")
        {
            map.Clear();

            GenerateBoss(map);
            GenerateGaps(map, roomcount);

            switch (spanningTreeAlgorithm)
            {
                case "PrimVariant":
                    GeneratePrimRoomVariant(map);
                    break;
                case "AldousBroder":
                    GenerateAldousBroder(map);
                    break;
                case "RandomEdges":
                    GenerateRandomEdges(map);
                    break;
                default:
                    GeneratePrim(map);
                    break;
            }

            if (spanningTreeAlgorithm != "RandomEdges")
            {
                AssignCritRooms(map);
                Rebase(map);
            }
        }

        // Generates boss and boss facing direction.
        public void GenerateBoss(Map map)
        {
            var boss = random.NextPoint(map.Width, map.Height);
            map.Base = map.Boss = boss;
            map.BossFaceDirection = random.Choice(boss.ValidDirections(map.Width, map.Height));
        }

        // Generates gaps.
        // This clears the map, and should be the first step in the generation process.
        public void GenerateGaps(Map map, int roomcount = 0)
        {
            if (roomcount <= 0)
                roomcount = random.RandomRoomcount(FloorSize);

            // If we have max rooms, we don't need gaps...
            if (roomcount >= map.MaxRooms)
                return;

            // This is for computational reasons.
            if (roomcount < FloorSize.MinRC)
                roomcount = FloorSize.MinRC;

            int attempts = 0;
            do
            {
                Logger.Log($"Starting gap generation", LogLevel.Trace);
                ++attempts;
                map.Clear();

                var points = MapUtils.Range2D(map.Width, map.Height).ToList();
                points.Remove(map.Boss);
                points.Remove(map.Boss.Add(map.BossFaceDirection));
                random.Shuffle(points);
                for (int i = 0; i < map.MaxRooms - roomcount; i++)
                    map[points[i]] = Direction.Gap;
            } while (!AreNonGapsConnected(map, roomcount));

            Logger.Log($"[{attempts} gap generation attempts to make contiguous map with {roomcount} rc]");
        }

        // Generates a completely random map with possible loops.
        public void GenerateRandomEdges(Map map)
        {
            var rooms = map.GetNonGaps();
            rooms.Remove(map.Boss);
            map.Base = random.Choice(rooms);

            foreach (var p in MapUtils.Range2D(map.Width, map.Height))
            {
                if (map[p] != Direction.Gap)
                {
                    map[p] = random.Choice((from dir in p.ValidDirections(map.Width, map.Height)
                                            where map[p.Add(dir)] != Direction.Gap
                                            select dir).ToList());
                }
            }
        }

        // Choose from open edges rather than rooms.
        public void GeneratePrim(Map map)
        {
            var openEnds = new List<(Point, Direction)> { (map.Base.Add(map.BossFaceDirection), map.BossFaceDirection.Flip()) };

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
                    if (CanExpandInto(map, p2))
                        openEnds.Add((p2, dir.Flip()));
                }
            }
        }

        // Select from rooms rather than doors.
        public void GeneratePrimRoomVariant(Map map)
        {
            var roomFromBase = map.Base.Add(map.BossFaceDirection);
            map[roomFromBase] = map.BossFaceDirection.Flip();
            var frontier = new List<Point>();
            AddNeighborsToFrontier(roomFromBase);

            while (frontier.Count > 0)
            {
                // Select random point in frontier and remove it
                var p = random.Choice(frontier);
                frontier.Remove(p);
                var validDirs = (from d in MapUtils.Directions
                                 let p2 = p.Add(d)
                                 where map.IsRoom(p2) && p2 != map.Base
                                 select d).ToArray();
                map[p] = random.Choice(validDirs);
                AddNeighborsToFrontier(p);
            }

            void AddNeighborsToFrontier(Point p)
            {
                foreach (var dir in ValidExpandDirections(map, p))
                {
                    var p2 = p.Add(dir);
                    if (!frontier.Contains(p2))
                        frontier.Add(p2);
                }
            }
        }

        // This makes a uniform distribution of spanning trees.
        public void GenerateAldousBroder(Map map)
        {
            // Random walk until map is full
            var visited = new HashSet<Point>(map.GetRooms());
            var p = map.Base.Add(map.BossFaceDirection);
            visited.Add(p);
            map[p] = map.BossFaceDirection.Flip();
            var targetCount = map.MaxRooms - map.GapCount;
            while (visited.Count < targetCount)
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

            // Assign some random (not-too-far) dead ends to crit until crit count exceeds desired.
            var deadEnds = map.GetDeadEnds().ToList();
            random.Shuffle(deadEnds);
            for (int i = 0; map.GetCritRooms().Count < desiredCritCount && i < deadEnds.Count; i++)
                map.AddCritEndpoint(deadEnds[i]);

            // Trim until crit count equals desired.
            while (map.GetCritRooms().Count > desiredCritCount)
            {
                var critEndpointList = map.CritEndpoints.ToList();
                map.BacktrackCritEndpoint(random.Choice(critEndpointList));
            }

            // Boss is crit.
            map.AddCritEndpoint(map.Boss);
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

        static bool AreNonGapsConnected(Map map, int roomcount)
        {
            // DFS, also time it
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif
            var stack = new Stack<Point>(new Point[] { map.Boss.Add(map.BossFaceDirection) });
            // Count boss as visited.
            var visited = new HashSet<Point> { map.Boss };
            var visitCount = 1;
            var maxStackCount = 0;

            while (stack.Count > 0)
            {
                if (maxStackCount < stack.Count)
                    maxStackCount = stack.Count;

                var p = stack.Pop();
                if (visited.Contains(p))
                    continue;

                visited.Add(p);
                ++visitCount;
                foreach (var dir in ValidExpandDirections(map, p))
                {
                    var p2 = p.Add(dir);
                    if (!visited.Contains(p2))
                        stack.Push(p2);
                }
            }

#if DEBUG
            sw.Stop();
            Logger.Log($"{roomcount - visitCount} unreachable rooms, max stack count={maxStackCount}, connectedness test took {sw.Elapsed.TotalMilliseconds} ms", LogLevel.Information);
#endif
            return visitCount >= roomcount;
        }

        static IEnumerable<Direction> ValidExpandDirections(Map map, Point p)
        {
            return from dir in p.ValidDirections(map.Width, map.Height) where CanExpandInto(map, p.Add(dir)) select dir;
        }

        // Is it not a gap?
        static bool CanExpandInto(Map map, Point p)
        {
            return p.IsInRange(map.Width, map.Height) && map[p] != Direction.Gap && !map.IsRoom(p);
        }
    }
}
