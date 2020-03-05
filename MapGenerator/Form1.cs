using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class Form1 : Form
    {
        private const int WIDTH = 8;
        private const int HEIGHT = 8;

        private Map map;

        public Form1()
        {
            InitializeComponent();
        }

        private void Generate1()
        {
            var parents = new Point[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    parents[x, y] = MapUtils.Invalid;
                }
            }
            var random = new Random();
            var agenda = new Queue<Point>();
            var home = new Point(random.Next(8), random.Next(8));
            agenda.Enqueue(home);
            while (agenda.Count > 0)
            {
                var curr = agenda.Dequeue();
                var neighbors = (from d in MapUtils.Offsets
                                 let p = Point.Add(curr, d)
                                 where MapUtils.IsInRange(p)
                                 select p).ToArray();
                neighbors.Shuffle();
                foreach (var n in neighbors)
                {
                    if (parents[n.X, n.Y] == MapUtils.Invalid && random.NextDouble() < (double)rcUpDown.Value)
                    {
                        parents[n.X, n.Y] = curr;
                        agenda.Enqueue(n);
                    }
                }
            }
        }

        private void Generate2(Direction[,] parents, Point baseLocation)
        {
            var random = new Random();
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
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

        // Busted: a path could trap itself.
        private void Generate3(Direction[,] parents, Point baseLocation, Point bossLocation)
        {
            var r = new Random();
            var pathIndex = new int[WIDTH, HEIGHT]; // 0: not visited, >0: visited
            pathIndex[baseLocation.X, baseLocation.Y] = 1;
            var agenda = (from y in Enumerable.Range(0, Height)
                          from x in Enumerable.Range(0, Width)
                          let p = new Point(x, y)
                          where p != baseLocation && p != bossLocation
                          select p).ToList();

            ProcessPoint(1, bossLocation);
            for (int i = 2; agenda.Count > 0; i++)
            {
                // Pick a random point and remove it
                var p = agenda[r.Next(agenda.Count)];
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
                        parents[p.X, p.Y] = validDirs[r.Next(validDirs.Length)];
                    else
                        break;
                    agenda.Remove(p);
                }
            }
        }

        private void Generate4(Direction[,] parents, Point baseLocation)
        {
            var r = new Random();
            var frontier = new List<Point>();
            AddNeighbors(baseLocation);
            while (frontier.Count > 0)
            {
                // Select random point in frontier and remove it
                var p = frontier[r.Next(frontier.Count)];
                frontier.Remove(p);
                var validDirs = (from d in MapUtils.Directions
                                 let p2 = p.Add(d)
                                 where p2 == baseLocation || (MapUtils.IsInRange(p2) && !frontier.Contains(p2) && parents[p2.X, p2.Y] != Direction.None)
                                 select d).ToArray();
                parents[p.X, p.Y] = validDirs[r.Next(validDirs.Length)];
                AddNeighbors(p);
            }

            void AddNeighbors(Point p)
            {
                foreach (var dir in MapUtils.Directions)
                {
                    var p2 = p.Add(dir);
                    if (MapUtils.IsInRange(p2) && p2 != baseLocation && !frontier.Contains(p2) && parents[p2.X, p2.Y] == Direction.None)
                        frontier.Add(p2);
                }
            }
        }

        // Sort by neighboring gaps
        private void RemoveDeadEnds1()
        {
            var r = new Random();
            List<Point> deadEnds = map.GetDeadEnds();

            for (int i = 0; i < 64 - (int)rcUpDown.Value; i++)
            {
                deadEnds = map.GetDeadEnds();
                deadEnds.Sort((a, b) => map.CountGapNeighbors(b) - map.CountGapNeighbors(a));
                var minGapNeighbors = map.CountGapNeighbors(deadEnds[0]);
                var myDeadEnds = (from d in deadEnds where map.CountGapNeighbors(d) == minGapNeighbors select d).ToList();
                var index = r.Next(myDeadEnds.Count);
                map.RemoveDeadEnd(myDeadEnds[index]);
            }
        }

        // Select dead ends at random
        private void RemoveDeadEnds2()
        {
            for (int i = 0; i < 64 - (int)rcUpDown.Value; i++)
            {
                RemoveDeadEnd();
            }
        }

        // Select current dead ends, only recompute if dead ends reaches 0
        private void RemoveDeadEnds3()
        {
            var r = new Random();
            var deadEnds = map.GetDeadEnds();

            for (int i = 0; i < 64 - (int)rcUpDown.Value; i++)
            {
                var index = r.Next(deadEnds.Count);
                map.RemoveDeadEnd(deadEnds[index]);
                deadEnds.RemoveAt(index);
                if (deadEnds.Count == 0)
                    deadEnds = map.GetDeadEnds();
            }
        }

        private void RemoveDeadEnd()
        {
            var r = new Random();
            var deadEnds = map.GetDeadEnds();
            if (deadEnds.Count > 0)
            {
                var index = r.Next(deadEnds.Count);
                map.RemoveDeadEnd(deadEnds[index]);
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            var r = new Random();
            var parents = new Direction[WIDTH, HEIGHT];
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    parents[x, y] = Direction.None;
                }
            }
            var baseLocation = new Point(r.Next(WIDTH), r.Next(HEIGHT));

            Generate4(parents, baseLocation);
            map = new Map(parents) { Base = baseLocation };

            RemoveDeadEnds3();

            var deadEnds = map.GetDeadEnds();
            map.Boss = deadEnds[r.Next(deadEnds.Count)];

            RenderMap();
        }

        private void RenderMap()
        {
            textBox1.Text = map.ToString();
            var bmp = map.ToImage();
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = bmp;
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox1.Image);
        }

        private void deleteDeadEndButton_Click(object sender, EventArgs e)
        {
            if (map != null)
            {
                RemoveDeadEnd();
                RenderMap();
            }
        }
    }
}
