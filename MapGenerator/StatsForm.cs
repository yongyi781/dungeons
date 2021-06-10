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
        MapGenerator generator = new(0, FloorSize.Large);
        readonly Random random = new();

        public StatsForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 5;
        }

        private void RunPerpendicularBaseTest(int trials = 1000)
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
                    UpdatePerpendicularBase(map, baseLoc, data);
                }
            }

            foreach (var kv in from pair in data orderby pair.Key.Y, pair.Key.X select pair)
            {
                var (t, tw, tn, te) = kv.Value;
                if (t > 0)
                {
                    Log($"Perpendicular base {kv.Key.ToChessString()}, maps={t}, w={(double)tw / t:f2}, n={(double)tn / t:f2}, e={(double)te / t:f2}");
                }
            }
        }

        private static void UpdatePerpendicularBase(Map map, Point baseLoc, Dictionary<Point, (int t, int tw, int tn, int te)> data)
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

        private void RunRowColumnGapTest(int roomcount, int trials = 10000)
        {
            var map = new Map(8, 8);
            int count = 0;
            for (int i = 0; i < trials; i++)
            {
                generator.GenerateBoss(map);
                generator.GenerateGaps(map, roomcount);

                if (HasFullRowColumnOrRowGap(map))
                    ++count;
            }
            Log($"rc={roomcount}, trials={trials}, p={(double)count / trials}{Environment.NewLine}");
        }

        private void RunDeadEndTest(int trials = 10000)
        {
            var map = new Map(8, 8);
            var deDist = new Dictionary<int, int>();
            var rrDist = new Dictionary<double, int>();
            for (int i = 0; i < trials; i++)
            {
                var seed = random.Next();
                map.Clear();
                generator = new MapGenerator(seed, FloorSize.Large);
                generator.Generate(map);

                var deCount = map.GetDeadEnds().Count;
                deDist[deCount] = deDist.GetValueOrDefault(deCount) + 1;

                var rr = map.Roomcount - 0.8 * deCount;
                rrDist[rr] = rrDist.GetValueOrDefault(rr) + 1;

                if (rr <= 34 || rr >= 52)
                    Log($"dead end count={deCount}, rc={map.Roomcount}, seed={seed}{Environment.NewLine}");
            }
            var pairs = from pair in rrDist
                        orderby pair.Key
                        select pair.Key + ":" + pair.Value;
            var str = "{" + string.Join(", ", pairs) + "}";
            Log(str + Environment.NewLine);
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

            Log(heights.Average() + Environment.NewLine);
            Log(bossEccs.Average() + Environment.NewLine);
            Log(diameters.Average() + Environment.NewLine);
            Log(string.Join(", ", heights) + "\r\n");
            Log(string.Join(", ", bossEccs) + "\r\n");
            Log(string.Join(", ", diameters) + "\r\n");
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

            Log($"{trials} trials");
            Log($"Wall bases = {(double)wallBases / trials}");
            Log($"Wall bosses = {(double)wallBosses / trials}");
            Log($"Wall gaps = {(double)wallGaps / totalGaps}");
            Log($"Wall dead ends = {(double)wallDeadEnds / totalDeadEnds}");
        }

        private void RealMatricesTest(string path)
        {
            if (!Directory.Exists(path))
            {
                Log("Directory not found." + Environment.NewLine);
                return;
            }

            var reader = new MapReader(Resources.ResourceManager);
            int count = 0;
            var rcs = new List<int>();
            var gapMatrix = new int[8, 8];
            var bossMatrix = new int[8, 8];
            var baseMatrix = new int[8, 8];
            var deMatrix = new int[8, 8];
            var maxDistsFromBase = new List<int>();
            var maxDistsFromBoss = new List<int>();
            var diameters = new List<int>();
            var wallBases = 0;
            var wallBosses = 0;
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
                        if (map.Base.IsOnWall(8, 8))
                            ++wallBases;
                        if (map.Boss.IsOnWall(8, 8))
                            ++wallBosses;
                        maxDistsFromBase.Add(map.GetTreeHeight());
                        maxDistsFromBoss.Add(map.GetEccentricity(map.Boss));
                    }
                }
            }
            Log($"Scoured {count} files\r\navg rc={rcs.Average()}");
            Log($"Base matrix:\r\n{baseMatrix.Normalize().ToPrettyString(a => a.ToString("f4"))}");
            Log($"Boss matrix:\r\n{bossMatrix.Normalize().ToPrettyString(a => a.ToString("f4"))}");
            Log($"Gap matrix:\r\n{gapMatrix.Normalize().ToPrettyString(a => a.ToString("f4"))}");
            Log($"Dead end matrix:\r\n{deMatrix.Normalize().ToPrettyString(a => a.ToString("f4"))}");
            Log($"Wall bases={(double)wallBases / count}");
            Log($"Avg max dist from base: {maxDistsFromBase.Average()}");
            Log($"Avg max dist from boss: {maxDistsFromBoss.Average()}");
        }

        private void RealBaseBossDistanceTest(string path)
        {
            if (!Directory.Exists(path))
            {
                Log("Directory not found." + Environment.NewLine);
                return;
            }

            int count = 0;
            var bossBaseDist = new double[24];
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
                        var dist = map.DistanceToBase(map.Boss);
                        if (dist >= 21)
                        {
                            Log($"Found interesting map: {Path.GetFileName(file)}");
                        }
                        ++bossBaseDist[map.DistanceToBase(map.Boss)];
                    }
                }
            }

            for (int i = 0; i < bossBaseDist.Length; i++)
                bossBaseDist[i] /= count;

            var average = Enumerable.Range(0, bossBaseDist.Length).Sum(i => i * bossBaseDist[i]);
            Log($"Scoured {count} files\r\nAverage boss-base distance: {average}");
            Log($"Base-boss distribution:\r\n{string.Join(", ", from i in Enumerable.Range(0, bossBaseDist.Length) select $"{i}:{bossBaseDist[i]:f4}")}");
        }

        private void Log(string text)
        {
            resultsTextBox.AppendText(text + Environment.NewLine);
        }

        private void RealPerpendicularBaseTest(string path)
        {
            if (!Directory.Exists(path))
            {
                Log("Directory not found." + Environment.NewLine);
                return;
            }

            int count = 0;
            var data = new Dictionary<Point, (int t, int tw, int tn, int te)>();
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
                        UpdatePerpendicularBase(map, map.Base, data);
                    }
                }
            }
            foreach (var kv in from pair in data orderby pair.Key.Y, pair.Key.X select pair)
            {
                var (t, tw, tn, te) = kv.Value;
                if (t > 0)
                {
                    Log($"Perpendicular base {kv.Key.ToChessString()}, maps={t}, w={(double)tw / t:f2}, n={(double)tn / t:f2}, e={(double)te / t:f2}{Environment.NewLine}");
                }
            }
        }

        private void RealNoWallGapTest(string path)
        {
            if (!Directory.Exists(path))
            {
                Log("Directory not found." + Environment.NewLine);
                return;
            }

            int total = 0;
            int count = 0;
            Dictionary<int, int> rcDist = new();
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
                        ++total;
                        var map = gameMap.Map;
                        if (!HasGapOnWall(map))
                        {
                            ++count;
                            rcDist[map.Roomcount] = rcDist.GetValueOrDefault(map.Roomcount) + 1;
                            if (map.Roomcount <= 64)
                            {
                                Log($"Found interesting map: {Path.GetFileName(file)}, rc = {map.Roomcount}");
                            }
                        }
                    }
                }
            }
            Log($"Scoured {total} files\r\nMaps with no wall gaps: {count}");
            var pairs = from pair in rcDist
                        orderby pair.Key
                        select pair.Key + ":" + pair.Value;
            var str = "{" + string.Join(", ", pairs) + "}";
            Log($"RC distribution: {str}");
        }

        private void RunBaseBossDistanceTest(int trials = 10000)
        {
            var map = new Map(8, 8);
            var bossBaseDist = new double[24];
            for (int i = 0; i < trials; i++)
            {
                generator.Generate(map);
                ++bossBaseDist[map.DistanceToBase(map.Boss)];
            }

            for (int i = 0; i < bossBaseDist.Length; i++)
                bossBaseDist[i] /= trials;

            var average = Enumerable.Range(0, bossBaseDist.Length).Sum(i => i * bossBaseDist[i]);
            Log($"Trials: {trials}\r\nAverage boss-base distance: {average}");
            Log($"Base-boss distribution:\r\n{string.Join(", ", from i in Enumerable.Range(0, bossBaseDist.Length) select $"{i}:{bossBaseDist[i]:f4}")}");
        }

        private void RunBaseMatrixTest(Point bossLoc, int trials = 10000)
        {
            var map = new Map(8, 8);
            var baseMatrix = new int[8, 8];
            for (int i = 0; i < trials; i++)
            {
                if (bossLoc == MapUtils.Invalid)
                    generator.GenerateBoss(map);
                else
                {
                    map.Base = map.Boss = bossLoc;
                    map.BossFaceDirection = random.Choice(bossLoc.ValidDirections(map.Width, map.Height));
                }
                generator.GenerateGaps(map);
                generator.GeneratePrim(map);
                generator.AssignCritRooms(map);

                var newBases = (from r in map.GetCritRooms()
                                where r != map.Boss
                                select r).ToList();
                foreach (var newBase in newBases)
                {
                    ++baseMatrix[newBase.X, newBase.Y];
                }
            }
            Log($"Base matrix:\r\n{baseMatrix.Normalize().ToPrettyString(a => a.ToString("f4"))}");
        }

        private void FindDuplicateAndGuideModeMaps(string path)
        {
            if (!Directory.Exists(path))
            {
                Log("Directory not found." + Environment.NewLine);
                return;
            }

            int count = 0;
            var mapStrs = new HashSet<string>();
            var reader = new MapReader(Resources.ResourceManager);
            foreach (var file in Directory.EnumerateFiles(path, "*.png"))
            {
                ++count;
                string mapStr;
                Map map;
                using (var bmp = new Bitmap(file))
                {
                    var floorSize = FloorSize.ByImageSize(bmp.Size);
                    var gameMap = reader.ReadMap(bmp, floorSize);
                    map = gameMap.Map;
                    mapStr = gameMap.Map.ToString();
                }
                if (map.CritEndpoints.Count > 0)
                {
                    // It's a guide mode map.
                    try
                    {
                        File.Move(file, Path.Combine(path, "Guide Mode", Path.GetFileName(file)));
                        Log($"Guide mode map moved: {Path.GetFileName(file)}");
                    }
                    catch (IOException)
                    {
                        Log($"Could not move guide mode map: {Path.GetFileName(file)}");
                    }
                }
                if (!mapStrs.Add(mapStr))
                {
                    // Duplicate.
                    try
                    {
                        File.Move(file, Path.Combine(path, "Duplicates", Path.GetFileName(file)));
                        Log($"Duplicate moved: {Path.GetFileName(file)}");
                    }
                    catch (IOException)
                    {
                        Log($"Could not move duplicate: {Path.GetFileName(file)}");
                    }
                }
            }
            Log($"Scoured {count} files");
        }

        private static bool HasFullRowColumnOrRowGap(Map map)
        {
            bool HasFullRowGap(int y)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[new Point(x, y)] != Direction.Gap)
                        return false;
                }
                return true;
            }

            bool HasFullColumnGap(int x)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map[new Point(x, y)] != Direction.Gap)
                        return false;
                }
                return true;
            }

            return HasFullRowGap(0) || HasFullRowGap(map.Height - 1) || HasFullColumnGap(0) || HasFullColumnGap(map.Width - 1);
        }

        private static bool HasGapOnWall(Map map)
        {
            bool HasRowGap(int y)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (map[new Point(x, y)] is Direction.Gap or Direction.None)
                        return true;
                }
                return false;
            }

            bool HasColumnGap(int x)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map[new Point(x, y)] is Direction.Gap or Direction.None)
                        return true;
                }
                return false;
            }

            return HasRowGap(0) || HasRowGap(map.Height - 1) || HasColumnGap(0) || HasColumnGap(map.Width - 1);
        }

        private void RunStatsTest(int test)
        {
            switch (test)
            {
                case 0:
                    if (!int.TryParse(textBox1.Text, out int trials))
                        trials = 1000;
                    RunPerpendicularBaseTest(trials);
                    break;
                case 1:
                    RunWallTest();
                    break;
                case 2:
                    RunBaseBossDistanceTest();
                    break;
                case 3:
                    RunBaseMatrixTest(MapUtils.ParseChess(textBox1.Text));
                    break;
                case 4:
                    RunHeightTest();
                    break;
                case 5:
                    RunDeadEndTest();
                    break;
                case 6:
                    if (int.TryParse(textBox1.Text, out int rc))
                        RunRowColumnGapTest(rc);
                    break;
                case 7:
                    RealPerpendicularBaseTest(textBox1.Text);
                    break;
                case 8:
                    RealBaseBossDistanceTest(textBox1.Text);
                    break;
                case 9:
                    RealMatricesTest(textBox1.Text);
                    break;
                case 10:
                    FindDuplicateAndGuideModeMaps(textBox1.Text);
                    break;
                case 11:
                    RealNoWallGapTest(textBox1.Text);
                    break;
                default:
                    Log("This shouldn't happen");
                    break;
            }
        }

        private async void runButton_Click(object sender, EventArgs e)
        {
            runButton.Enabled = false;
            var index = comboBox1.SelectedIndex;
            await Task.Run(() => RunStatsTest(index));
            runButton.Enabled = true;
        }
    }
}
