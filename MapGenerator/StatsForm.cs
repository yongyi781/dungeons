using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class StatsForm : Form
    {
        MapGenerator generator = new MapGenerator(0, FloorSize.Large);
        readonly Random random = new Random();

        public StatsForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 5;
        }

        private void RunPerpendicularBaseTest(int trials = 10000)
        {
            var map = new Map(8, 8);

            var data = new Dictionary<Point, (int t, int tw, int tn, int te)>();
            for (int i = 0; i < trials; i++)
            {
                generator.GenerateBoss(map);
                generator.GenerateGaps(map);
                generator.GeneratePrim(map);

                foreach (var baseLoc in map.GetRooms())
                {
                    int t = 0, tw = 0, tn = 0, te = 0;
                    map.Rebase(baseLoc);

                    var dirs = map.ChildrenDirs(map.Base).ToList();
                    if (dirs.Count == 3 && dirs.Contains(Direction.W) && dirs.Contains(Direction.N) && dirs.Contains(Direction.E))
                    {
                        ++t;
                        var ws = map.SubtreeSize(map.Base.Add(Direction.W));
                        var ns = map.SubtreeSize(map.Base.Add(Direction.N));
                        var es = map.SubtreeSize(map.Base.Add(Direction.E));
                        var max = Math.Max(ws, Math.Max(ns, es));

                        var wsc = ws == max ? 1 : 0;
                        tw += wsc;
                        var nsc = ns == max ? 1 : 0;
                        tn += nsc;
                        var esc = es == max ? 1 : 0;
                        te += esc;
                    }
                    var item = data.GetValueOrDefault(baseLoc);
                    item.t += t;
                    item.tw += tw;
                    item.tn += tn;
                    item.te += te;
                    data[baseLoc] = item;
                }
            }

            foreach (var kv in from pair in data orderby pair.Key.Y orderby pair.Key.X select pair)
            {
                var (t, tw, tn, te) = kv.Value;
                if (t > 0)
                {
                    resultsTextBox.AppendText($"Perpendicular base {kv.Key.ToChessString()}, maps={t}, w={(double)tw / t:f2}, n={(double)tn / t:f2}, e={(double)te / t:f2}{Environment.NewLine}");
                }
            }
        }

        private void RunRowColumnGapTest(int roomcount, int trials = 10000)
        {
            var map = new Map(8, 8);
            int count = 0;
            for (int i = 0; i < trials; i++)
            {
                generator.GenerateBoss(map);
                generator.GenerateGaps(map, roomcount);

                if (HasColumnOrRowGap(map))
                    ++count;
            }
            resultsTextBox.AppendText($"rc={roomcount}, trials={trials}, p={(double)count / trials}{Environment.NewLine}");
        }

        private void RunDeadEndTest(int trials = 10000)
        {
            var map = new Map(8, 8);
            var dist = new Dictionary<int, int>();
            for (int i = 0; i < trials; i++)
            {
                var seed = random.Next();
                map.Clear();
                generator = new MapGenerator(seed, FloorSize.Large);
                generator.Generate(map);

                var deCount = map.GetDeadEnds().Count;
                dist[deCount] = dist.GetValueOrDefault(deCount) + 1;

                if (deCount <= 12 || deCount >= 26)
                    resultsTextBox.AppendText($"dead end count={deCount}, rc={map.Roomcount}, seed={seed}{Environment.NewLine}");
            }
            var pairs = from pair in dist
                        orderby pair.Key
                        select pair.Key + ": " + pair.Value;
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
                generator.Generate(map);

                var value = map.GetTreeHeight();
                heights.Add(value);
                bossEccs.Add(map.GetEccentricity(map.Boss));
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
                generator.Generate(map);
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
            //var gapMatrix = new int[8, 8];
            //var bossMatrix = new int[8, 8];
            var baseMatrix = new int[8, 8];
            //var deMatrix = new int[8, 8];
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
                    }
                }
            }
            resultsTextBox.AppendText($"Scoured {count} files\r\navg rc={rcs.Average()}\r\n");
            resultsTextBox.AppendText($"Base matrix:\r\n{baseMatrix.Normalize().ToPrettyString(a => a.ToString("f4"))}\r\n");
            //resultsTextBox.AppendText($"Boss matrix:\r\n{bossMatrix.ToPrettyString(a => a.ToString())}\r\n");
            //resultsTextBox.AppendText($"Gap matrix:\r\n{gapMatrix.ToPrettyString(a => a.ToString())}\r\n");
            //resultsTextBox.AppendText($"Dead end matrix:\r\n{deMatrix.ToPrettyString(a => a.ToString())}\r\n");
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

        private void RunBaseBossDistanceTest(int trials = 10000)
        {
            var map = new Map(8, 8);
            var baseBossDistances = new List<int>();
            for (int i = 0; i < trials; i++)
            {
                generator.Generate(map);
                baseBossDistances.Add(map.DistanceToBase(map.Boss));
            }

            resultsTextBox.AppendText(baseBossDistances.Average() + Environment.NewLine);
            resultsTextBox.AppendText(string.Join(", ", baseBossDistances) + "\r\n\r\n");
        }

        private void RunBaseMatrixTest(int trials = 10000)
        {
            var map = new Map(8, 8);
            var baseMatrix = new int[8, 8];
            for (int i = 0; i < trials; i++)
            {
                generator.Generate(map);
                ++baseMatrix[map.Base.X, map.Base.Y];
            }
            resultsTextBox.AppendText($"Base matrix:\r\n{baseMatrix.Normalize().ToPrettyString(a => a.ToString("f4"))}\r\n");
        }

        private bool HasColumnOrRowGap(Map map)
        {
            bool HasRowGap(int y)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[new Point(x, y)] != Direction.Gap)
                        return false;
                }
                return true;
            }

            bool HasColumnGap(int x)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map[new Point(x, y)] != Direction.Gap)
                        return false;
                }
                return true;
            }

            return HasRowGap(0) || HasRowGap(map.Height - 1) || HasColumnGap(0) || HasColumnGap(map.Width - 1);
        }

        private async void runButton_Click(object sender, EventArgs e)
        {
            runButton.Enabled = false;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    await Task.Run(() => RunPerpendicularBaseTest());
                    break;
                case 1:
                    if (int.TryParse(textBox1.Text, out int rc))
                        await Task.Run(() => RunRowColumnGapTest(rc));
                    break;
                case 2:
                    RunDeadEndTest();
                    break;
                case 3:
                    RunHeightTest();
                    break;
                case 4:
                    RunReadFromFolderTest(textBox1.Text);
                    break;
                case 5:
                    RunWallTest();
                    break;
                case 6:
                    RunBaseBossDistanceTest();
                    break;
                case 7:
                    RunBaseMatrixTest();
                    break;
                default:
                    break;
            }
            runButton.Enabled = true;
        }
    }
}
