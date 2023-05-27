using Dungeons.Common;

namespace MapGenerator
{
    public partial class Form1 : Form
    {
        private readonly MapWriter mapWriter = new MapWriter(Resources.ResourceManager);
        private readonly Random random = new Random();
        private readonly string[] Algorithms = { "Prim", "AldousBroder", "PrimVariant", "RandomEdges" };
        private readonly FloorSize[] FloorSizes = { FloorSize.Small, FloorSize.Medium, FloorSize.Large, new FloorSize(12, 12), new FloorSize(16, 16) };

        private Map? map;
        private MapGenerator? mapGenerator;

        public Form1()
        {
            InitializeComponent();

            algorithmComboBox.SelectedIndex = 0;
            sizeComboBox.SelectedIndex = 2;
            UpdateSize();

            Logger.Global.TextBox = logTextBox;
#if DEBUG
            Logger.Global.LogLevel = LogLevel.Debug;
            sizeComboBox.SelectedIndex = sizeComboBox.Items.Count - 1;
            sizeUpDown.Value = 64;
            rcUpDown.Value = 64 * 64 - 1;
#else
            Logger.Global.LogLevel = LogLevel.Information;
#endif
        }

        private void Generate(int roomcount, string algorithm)
        {
            if (map == null)
                throw new InvalidOperationException("map is null.");
            mapGenerator = new MapGenerator((int)seedUpDown.Value, new FloorSize(map.Width, map.Height));
            mapGenerator.Generate(map, roomcount, algorithm);

            Logger.Global.Log(LogLevel.Information, $"size={mapGenerator.FloorSize}, seed={seedUpDown.Value,10}, rc={map.Roomcount}, alg={algorithm}, crc={map.GetCritRooms().Count}, dead ends={map.GetDeadEnds().Count}, boss-base dist={map.DistanceToBase(map.Boss)}, length from base={map.GetTreeHeight()}");
        }

        private void RenderMap()
        {
            if (map == null)
                throw new InvalidOperationException("map is null.");
            textBox1.Text = map.ToString();
            using (var g = Graphics.FromImage(pictureBox.Image))
            {
                Logger.Global.Log(LogLevel.Information, pictureBox.Image.Size.ToString());
                mapWriter.Draw(g, map, drawCritCheckBox.Checked, drawDeadEndsCheckBox.Checked);
            }
            pictureBox.Refresh();
        }

        private void UpdateSize()
        {
            var index = sizeComboBox.SelectedIndex;
            var size = index == sizeComboBox.Items.Count - 1 ? new FloorSize((int)sizeUpDown.Value, (int)sizeUpDown.Value) : FloorSizes[index];

            pictureBox.Width = LogicalToDeviceUnits(size.Width * MapUtils.RoomSize + 24);
            pictureBox.Height = LogicalToDeviceUnits(size.Height * MapUtils.RoomSize + 24);
            if (map?.Size != size.Size)
            {
                map = new Map(size.Width, size.Height);
                pictureBox.Image = new Bitmap(size.Width * MapUtils.RoomSize, size.Height * MapUtils.RoomSize);
            }
        }

        private async void GenerateButton_Click(object sender, EventArgs e)
        {
            try
            {
                generateButton.Enabled = false;

                UpdateSize();

                if (randomSeedCheckbox.Checked)
                    seedUpDown.Value = random.Next();

                var rc = randomRcCheckbox.Checked ? 0 : (int)rcUpDown.Value;
                var alg = Algorithms[algorithmComboBox.SelectedIndex];
                await Task.Run(() => Generate(rc, alg));
                if (map != null)
                    rcUpDown.Value = map.Roomcount;
                RenderMap();
            }
            finally
            {
                generateButton.Enabled = true;
            }
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox.Image);
        }

        private void DeleteDeadEndButton_Click(object sender, EventArgs e)
        {
            if (map != null)
            {
                foreach (var point in map.GetBonusDeadEnds())
                    map.RemoveDeadEnd(point);
                RenderMap();
            }
        }

        private void DrawBox_CheckedChanged(object sender, EventArgs e)
        {
            RenderMap();
        }

        private void OpenStatsWindowButton_Click(object sender, EventArgs e)
        {
            new StatsForm().Show(this);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Image Files|*.png|All Files|*.*" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var bmp = new Bitmap(ofd.FileName);
                var gameMap = new MapReader(Resources.ResourceManager).ReadMap(bmp, FloorSize.ByImageSize(bmp.Size));
                sizeComboBox.SelectedIndex = Array.IndexOf(FloorSizes, gameMap.FloorSize);
                UpdateSize();
                map = gameMap.Map;
                RenderMap();

                var subtreeStr = string.Empty;
                foreach (var dir in map.ChildrenDirs(map.Base))
                {
                    subtreeStr += $", {dir}={map.SubtreeSize(map.Base.Add(dir))}";
                }
                Logger.Global.Log(LogLevel.Information, $"{(gameMap.IsComplete ? "[complete]   " : "[incomplete] ")}name={Path.GetFileName(ofd.FileName)}, rc={map.Roomcount}, dead ends={map.GetDeadEnds().Count,2}, boss-base dist={map.DistanceToBase(map.Boss)}, length from base={map.GetTreeHeight()}{subtreeStr}");
            }
        }

        private void SizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            sizeUpDown.Enabled = sizeComboBox.SelectedIndex == sizeComboBox.Items.Count - 1;
        }

        private void pictureBox_DpiChangedAfterParent(object sender, EventArgs e)
        {
            UpdateSize();
        }
    }
}
