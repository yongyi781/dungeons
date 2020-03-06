using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public class MapGenerator
    {
        private Random random;

        public MapGenerator(int seed, int minCrit, int maxCrit)
        {
            random = new Random(seed);
            MinCrit = minCrit;
            MaxCrit = maxCrit;
        }

        public int MinCrit { get; set; }
        public int MaxCrit { get; set; }

        public void GenerateBaseLocation(Map map)
        {
            map.Base = random.NextPoint(map.Width, map.Width);
        }

        // Generates a completely random map with possible loops.
        public async Task GenerateWithPossibleLoops(Map map, Func<Task> callback)
        {
            foreach (var p in MapUtils.GridPoints(map.Width, map.Height))
            {
                if (p != map.Base)
                {
                    map[p] = random.Choice(p.ValidDirections(map.Width, map.Height));

                    if (callback != null)
                        await callback.Invoke();
                }
            }
        }

        public async Task GeneratePrim(Map map, Func<Task> callback = null)
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

                if (callback != null)
                {
                    await callback.Invoke();
                    Logger.Log(string.Join('|', from f in frontier select f.ToShortString()));
                }
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

        // Choose from open edges rather than rooms.
        public async Task GeneratePrimVariant(Map map, Func<Task> callback)
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

                if (callback != null)
                    await callback();
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

        // This makes a uniform distribution of spanning trees.
        public async Task GenerateAldousBroder(Map map, Func<Task> callback = null)
        {
            // Random walk until map is full
            var visited = new HashSet<Point>(map.GetRooms());
            var p = map.Base;
            while (visited.Count < map.Width * map.Height)
            {
                var validDirs = p.ValidDirections(map.Width, map.Height);
                var dir = random.Choice(validDirs);
                p = p.Add(dir);
                if (!visited.Contains(p))
                {
                    visited.Add(p);
                    map[p] = dir.Flip();
                    if (callback != null)
                        await callback();
                }
            }
        }

        public void AssignBossAndCritRooms(Map map)
        {
            var deadEnds = map.GetDeadEnds();
            random.Shuffle(deadEnds);
            for (int i = 0; map.CritRooms.Count < MinCrit - 1 && i < deadEnds.Count; i++)
            {
                map.AddCritRoom(deadEnds[i]);
            }

            map.Boss = deadEnds[0];
        }

        // Select dead ends at random
        public async Task MakeGaps(Map map, int roomcount, Func<Task> callback = null)
        {
            for (int i = 0; i < map.MaxRooms - roomcount; i++)
            {
                RemoveBonusDeadEnd(map);
                if (callback != null)
                    await callback();
            }
        }

        public void RemoveBonusDeadEnd(Map map)
        {
            // Remove a random turning dead end if possible, otherwise remove any dead end.
            var deadEnds = map.GetBonusDeadEnds();
            //if (deadEnds.Count == 0)
            //    deadEnds = map.GetDeadEnds(false);
            if (deadEnds.Count > 0)
                map.RemoveDeadEnd(random.Choice(deadEnds));
            //map.RemoveRoom(map.FindDeadEndByRandomWalk());
        }

        public Point FindDeadEndByRandomWalk(Map map)
        {
            var p = map.Base;
            while (!map.IsDeadEnd(p))
            {
                Logger.Log("Random walk yields " + p);
                var n = map.ChildrenDirs(p).ToList();
                p = p.Add(random.Choice(n));
            }
            return p;
        }
    }
}
