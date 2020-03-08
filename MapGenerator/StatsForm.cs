using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class StatsForm : Form
    {
        MapGenerator generator = new MapGenerator(0, FloorSize.Large);
        Random random = new Random();

        public StatsForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 5;
        }

        private void RunPerpendicularBaseTest(Point baseLoc, int trials = 10000)
        {
            var map = new Map(8, 8);

            int tw = 0, tn = 0, te = 0;

            for (int i = 0; i < trials; i++)
            {
                map.Clear();
                map.Base = baseLoc;
                if (map.Base == MapUtils.Invalid)
                    generator.GenerateBaseLocation(map);
                var wp = map.Base.Add(Direction.W);
                var np = map.Base.Add(Direction.N);
                var ep = map.Base.Add(Direction.E);

                if (wp.IsInRange(8, 8))
                    map[wp] = Direction.E;
                if (np.IsInRange(8, 8))
                    map[np] = Direction.S;
                if (ep.IsInRange(8, 8))
                    map[ep] = Direction.W;
                generator.GeneratePrim(map);
                var ws = map.SubtreeSize(wp);
                var ns = map.SubtreeSize(np);
                var es = map.SubtreeSize(ep);
                var max = Math.Max(ws, Math.Max(ns, es));

                var wsc = ws == max ? 1 : 0;
                tw += wsc;
                var nsc = ns == max ? 1 : 0;
                tn += nsc;
                var esc = es == max ? 1 : 0;
                te += esc;
            }

            resultsTextBox.AppendText($"Perpendicular base {baseLoc.ToChessString()}, trials={trials}, w={(double)tw / trials}, n={(double)tn / trials}, e={(double)te / trials}{Environment.NewLine}");
        }

        private void RunRowColumnGapTest(int roomcount, int trials = 10000)
        {
            var map = new Map(8, 8);
            int count = 0;
            for (int i = 0; i < trials; i++)
            {
                map.Clear();
                generator.GenerateBaseLocation(map);
                generator.GeneratePrim(map);
                generator.AssignCritRooms(map);
                generator.MakeGaps(map, roomcount);

                if (HasColumnOrRowGap(map))
                    ++count;
            }
            resultsTextBox.AppendText($"rc={roomcount}, trials={trials}, p={(double)count / trials}{Environment.NewLine}");
        }

        private void RunDeadEndTest(int trials = 100000)
        {
            var map = new Map(8, 8);
            var dist = new Dictionary<int, int>();
            for (int i = 0; i < trials; i++)
            {
                var seed = random.Next();
                map.Clear();
                generator = new MapGenerator(seed, FloorSize.Large);
                generator.GenerateBaseLocation(map);
                generator.GeneratePrim(map);

                var deCount = map.GetDeadEnds().Count;
                dist[deCount] = dist.GetValueOrDefault(deCount) + 1;

                if (deCount <= 12 || deCount >= 30)
                    resultsTextBox.AppendText($"seed={seed}, dead end count={deCount}{Environment.NewLine}");
            }
            var pairs = from pair in dist
                        orderby pair.Key
                        select pair.Key + "=" + pair.Value;
            var str = "{" + string.Join(",", pairs) + "}";
            resultsTextBox.AppendText(str + Environment.NewLine);
        }

        private void RunHeightTest(int trials = 5000)
        {
            var map = new Map(8, 8);
            var heights = new List<int>();
            var bossEccs = new List<int>();
            var diameters = new List<int>();
            for (int i = 0; i < trials; i++)
            {
                var seed = random.Next();
                map.Clear();
                generator = new MapGenerator(seed, FloorSize.Large);
                generator.GenerateBaseLocation(map);
                generator.GeneratePrim(map);

                var value = map.GetTreeHeight();
                heights.Add(value);
                bossEccs.Add(map.GetEccentricity(random.Choice(map.GetRooms())));
                diameters.Add(map.GetDiameter());
            }

            resultsTextBox.AppendText(heights.Average() + Environment.NewLine);
            resultsTextBox.AppendText(bossEccs.Average() + Environment.NewLine);
            resultsTextBox.AppendText(diameters.Average() + Environment.NewLine);
            resultsTextBox.AppendText(string.Join(", ", heights) + "\r\n\r\n");
            resultsTextBox.AppendText(string.Join(", ", bossEccs) + "\r\n\r\n");
            resultsTextBox.AppendText(string.Join(", ", diameters) + "\r\n\r\n");
        }

        private void RunWallTest(int trials = 10000)
        {
            var map = new Map(8, 8);
            var wallBases = 0;
            var wallGaps = 0;
            var totalGaps = 0;
            var wallBosses = 0;
            var wallDeadEnds = 0;
            var totalDeadEnds = 0;
            for (int i = 0; i < trials; i++)
            {
                var seed = random.Next();
                map.Clear();
                generator = new MapGenerator(seed, FloorSize.Large);
                generator.GenerateBaseLocation(map);
                generator.GeneratePrim(map);
                generator.AssignCritRooms(map);
                generator.MakeGaps(map);
                if (map.Base.IsOnWall(8, 8))
                    ++wallBases;
                if (map.Boss.IsOnWall(8, 8))
                    ++wallBosses;
                var deadEndList = map.GetDeadEnds();
                wallDeadEnds += deadEndList.Where(g => g.IsOnWall(8, 8)).Count();
                totalDeadEnds += deadEndList.Count;

                var gapList = map.GetNonRooms();
                wallGaps += gapList.Where(g => g.IsOnWall(8, 8)).Count();
                totalGaps += gapList.Count;
            }

            resultsTextBox.AppendText($"{trials} trials\r\n");
            resultsTextBox.AppendText($"Wall bases = {(double)wallBases / trials}\r\n");
            resultsTextBox.AppendText($"Wall bosses = {(double)wallBosses / trials}\r\n");
            resultsTextBox.AppendText($"Wall gaps = {(double)wallGaps / totalGaps}\r\n");
            resultsTextBox.AppendText($"Wall dead ends = {(double)wallDeadEnds / totalDeadEnds}\r\n");
        }

        private void RunReadFromFolderTest(string path)
        {
            if (!Directory.Exists(path))
            {
                resultsTextBox.AppendText("Directory not found." + Environment.NewLine);
                return;
            }

            var reader = new MapReader(Resources.ResourceManager);
            int count = 0;
            var rcs = new List<int>();
            var gapMatrix = new int[8, 8];
            var bossMatrix = new int[8, 8];
            var baseMatrix = new int[8, 8];
            var deMatrix = new int[8, 8];
            //var treeHeights = new List<int>();
            //var bossEccs = new List<int>();
            //var diameters = new List<int>();
            //var countWallBosses = 0;
            //var wallDes = 0;
            //var totalDes = 0;
            foreach (var file in Directory.EnumerateFiles(path, "*.png"))
            {
                var bmp = new Bitmap(file);
                var floorSize = FloorSize.ByImageSize(bmp.Size);
                if (floorSize == FloorSize.Large)
                {
                    var gameMap = reader.ReadMap(bmp, floorSize);
                    if (gameMap.OpenedRoomCount >= 50 && gameMap.IsComplete)
                    {
                        ++count;
                        var map = gameMap.Map;
                        rcs.Add(gameMap.OpenedRoomCount);

                        ++baseMatrix[map.Base.X, map.Base.Y];
                        ++bossMatrix[map.Boss.X, map.Boss.Y];
                        var deadEndList = map.GetDeadEnds();
                        foreach (var d in deadEndList)
                            ++deMatrix[d.X, d.Y];

                        var gapList = map.GetNonRooms();
                        foreach (var g in gapList)
                            ++gapMatrix[g.X, g.Y];
                    }
                }
            }
            resultsTextBox.AppendText($"Scoured {count} files\r\navg rc={rcs.Average()}\r\n");
            resultsTextBox.AppendText($"Base matrix:\r\n{baseMatrix.ToPrettyString(a => a.ToString())}\r\n");
            resultsTextBox.AppendText($"Boss matrix:\r\n{bossMatrix.ToPrettyString(a => a.ToString())}\r\n");
            resultsTextBox.AppendText($"Gap matrix:\r\n{gapMatrix.ToPrettyString(a => a.ToString())}\r\n");
            resultsTextBox.AppendText($"Dead end matrix:\r\n{deMatrix.ToPrettyString(a => a.ToString())}\r\n");
            //resultsTextBox.AppendText($"Wall bases={(double)countWallBosses / count}\r\n");
            //resultsTextBox.AppendText($"Average wall dead ends={(double)wallDes / totalDes}\r\n");
            //resultsTextBox.AppendText(string.Join(", ", treeHeights) + "\r\n\r\n");
            //resultsTextBox.AppendText(string.Join(", ", bossEccs) + "\r\n\r\n");
            //resultsTextBox.AppendText(string.Join(", ", diameters));
        }

        private void RealBaseBossDistanceTest(string path)
        {
            if (!Directory.Exists(path))
            {
                resultsTextBox.AppendText("Directory not found." + Environment.NewLine);
                return;
            }

            int count = 0;
            var baseBossDistances = new List<int>();
            var reader = new MapReader(Resources.ResourceManager);
            foreach (var file in Directory.EnumerateFiles(path, "*.png"))
            {
                var bmp = new Bitmap(file);
                var floorSize = FloorSize.ByImageSize(bmp.Size);
                if (floorSize == FloorSize.Large)
                {
                    var gameMap = reader.ReadMap(bmp, floorSize);
                    if (gameMap.OpenedRoomCount >= 50 && gameMap.IsComplete)
                    {
                        ++count;
                        var map = gameMap.Map;
                        baseBossDistances.Add(map.DistanceToBase(map.Boss));
                    }
                }
            }
            resultsTextBox.AppendText($"Scoured {count} files\r\n");
            resultsTextBox.AppendText($"Base-boss distances:\r\n{string.Join(",", baseBossDistances)}\r\n");
        }

        private bool HasColumnOrRowGap(Map map)
        {
            bool HasRowGap(int y)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[new Point(x, y)] != Direction.None)
                        return false;
                }
                return true;
            }

            bool HasColumnGap(int x)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map[new Point(x, y)] != Direction.None)
                        return false;
                }
                return true;
            }

            return HasRowGap(0) || HasRowGap(map.Height - 1) || HasColumnGap(0) || HasColumnGap(map.Width - 1);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    RunPerpendicularBaseTest(MapUtils.ParseChess(textBox1.Text));
                    break;
                case 1:
                    if (int.TryParse(textBox1.Text, out int rc))
                        RunRowColumnGapTest(rc);
                    break;
                case 2:
                    RunDeadEndTest();
                    break;
                case 3:
                    RunHeightTest();
                    break;
                case 4:
                    RealBaseBossDistanceTest(textBox1.Text);
                    break;
                case 5:
                    RunWallTest();
                    break;
                default:
                    break;
            }
        }
    }
}
