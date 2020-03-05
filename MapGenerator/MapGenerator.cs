using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MapGenerator
{
    public static class MapGenerator
    {
        private static Random random = new Random();

        public static void SetSeed(int seed)
        {
            random = new Random(seed);
        }

        public static void Generate2(Direction[,] parents, Point baseLocation)
        {
            for (int y = 0; y < parents.GetLength(1); y++)
            {
                for (int x = 0; x < parents.GetLength(0); x++)
                {
                    Direction dir = Direction.None;
                    Point p2;
                    int i = 0;
                    do
                    {
                        dir = MapUtils.Directions[random.Next(MapUtils.Directions.Length)];
                        p2 = MapUtils.Add(new Point(x, y), dir);
                        ++i;
                    } while (i < 100 && (!MapUtils.IsInRange(p2) || parents[p2.X, p2.Y] == MapUtils.Flip(dir)));
                    parents[x, y] = dir;
                }
            }
        }

        // Wilson's loop-erasing path algorithm
        public static void Generate3(Direction[,] parents, Point baseLocation, Point bossLocation)
        {
            var pathIndex = new int[parents.GetLength(0), parents.GetLength(1)]; // 0: not visited, >0: visited
            pathIndex[baseLocation.X, baseLocation.Y] = 1;
            var agenda = (from y in Enumerable.Range(0, parents.GetLength(0))
                          from x in Enumerable.Range(0, parents.GetLength(1))
                          let p = new Point(x, y)
                          where p != baseLocation && p != bossLocation
                          select p).ToList();

            ProcessPoint(1, bossLocation);
            for (int i = 2; agenda.Count > 0; i++)
            {
                // Pick a random point and remove it
                var p = agenda[random.Next(agenda.Count)];
                ProcessPoint(i, p);
            }

            void ProcessPoint(int i, Point point)
            {
                for (var p = point; p != baseLocation; p = MapUtils.Add(p, parents[p.X, p.Y]))
                {
                    // Random walk until you hit base or a previous path.
                    pathIndex[p.X, p.Y] = i;
                    var validDirs = (from d in MapUtils.Directions
                                     let p2 = MapUtils.Add(p, d)
                                     where MapUtils.IsInRange(p2)
                                     where pathIndex[p2.X, p2.Y] != i
                                     select d).ToArray();
                    if (validDirs.Length > 0)
                        parents[p.X, p.Y] = validDirs[random.Next(validDirs.Length)];
                    else
                        break;
                    agenda.Remove(p);
                }
            }
        }

        public static Map Generate4(int roomcount)
        {
            const int WIDTH = 8, HEIGHT = 8;

            var parents = new Direction[WIDTH, HEIGHT];
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    parents[x, y] = Direction.None;
                }
            }

            var baseLocation = new Point(random.Next(WIDTH), random.Next(HEIGHT));

            var frontier = new List<Point>();
            AddNeighborsToFrontier(baseLocation);
            while (frontier.Count > 0)
            {
                // Select random point in frontier and remove it
                var p = frontier[random.Next(frontier.Count)];
                frontier.Remove(p);
                var validDirs = (from d in MapUtils.Directions
                                 let p2 = p.Add(d)
                                 where p2 == baseLocation || (MapUtils.IsInRange(p2) && parents[p2.X, p2.Y] != Direction.None)
                                 select d).ToArray();
                parents[p.X, p.Y] = validDirs[random.Next(validDirs.Length)];
                AddNeighborsToFrontier(p);
            }

            var map = new Map(parents) { Base = baseLocation };
            var deadEnds = map.GetDeadEnds();
            deadEnds.Shuffle(random);
            for (int i = 0; map.CritRooms.Count < 18 && i < deadEnds.Count; i++)
            {
                map.AddCritRoom(deadEnds[i]);
            }

            map.Boss = deadEnds[0];

            MakeGaps2(map, roomcount);
            return map;

            void AddNeighborsToFrontier(Point p)
            {
                foreach (var dir in MapUtils.Directions)
                {
                    var p2 = p.Add(dir);
                    if (MapUtils.IsInRange(p2) && p2 != baseLocation && !frontier.Contains(p2) && parents[p2.X, p2.Y] == Direction.None)
                        frontier.Add(p2);
                }
            }
        }

        // Select dead ends at random
        public static void MakeGaps2(Map map, int roomcount)
        {
            for (int i = 0; i < 64 - roomcount; i++)
            {
                RemoveDeadEnd(map);
            }
        }

        // Select current dead ends, only recompute if dead ends reaches 0
        public static void MakeGaps3(Map map, int roomcount)
        {
            var deadEnds = map.GetBonusDeadEnds();

            for (int i = 0; i < 64 - roomcount; i++)
            {
                var index = random.Next(deadEnds.Count);
                map.RemoveRoom(deadEnds[index]);
                deadEnds.RemoveAt(index);
                if (deadEnds.Count == 0)
                    deadEnds = map.GetBonusDeadEnds();
            }
        }

        // Select current dead ends, only recompute if dead ends reaches 0
        public static void MakeGaps4(Map map, int roomcount)
        {

            for (int i = 0; i < 64 - roomcount; i++)
            {
                var deadEnds = map.GetDeadEnds();
                deadEnds.Shuffle(random);
                var max = deadEnds.Max(a => map.GetDensity(a));
                var first = deadEnds.First(a => map.GetDensity(a) == max);
                map.RemoveRoom(first);
            }
        }

        public static void RemoveDeadEnd(Map map)
        {
            // Remove a random turning dead end if possible, otherwise remove any dead end.
            var deadEnds = map.GetBonusDeadEnds();
            //if (deadEnds.Count == 0)
            //    deadEnds = map.GetDeadEnds(false);
            if (deadEnds.Count > 0)
            {
                var index = random.Next(deadEnds.Count);
                map.RemoveRoom(deadEnds[index]);
            }
            //map.RemoveRoom(map.FindDeadEndByRandomWalk());
        }

        public static Point FindDeadEndByRandomWalk(Map map)
        {
            var p = map.Base;
            while (!map.IsDeadEnd(p))
            {
                Logger.Log("Random walk yields " + p);
                var n = map.GetNeighbors(p);
                p = n[random.Next(n.Count)];
            }
            return p;
        }
    }
}
