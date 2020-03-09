using Dungeons.Common;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class Form1 : Form
    {
        private readonly MapWriter mapWriter = new MapWriter(Resources.ResourceManager);
        private readonly Random random = new Random();
        private readonly string[] Algorithms = { "Prim", "AldousBroder", "PrimVariant", "RandomEdges" };
        private Map map;
        private MapGenerator mapGenerator;

        public Form1()
        {
            InitializeComponent();

            algorithmComboBox.SelectedIndex = 0;
            sizeComboBox.SelectedIndex = 2;

            Logger.TextBox = logTextBox;
        }

        private void Generate(int roomcount)
        {
            mapGenerator = new MapGenerator((int)seedUpDown.Value, FloorSize.ByDimensions(map.Width, map.Height));
            mapGenerator.Generate(map, roomcount, Algorithms[algorithmComboBox.SelectedIndex]);

            var subtreeStr = string.Empty;
            foreach (var dir in map.ChildrenDirs(map.Base))
            {
                subtreeStr += $", {dir}={map.SubtreeSize(map.Base.Add(dir))}";
            }

            Logger.Log($"seed={seedUpDown.Value,10}, rc={map.Roomcount}, crit count={map.GetCritRooms().Count}, dead end count={map.GetDeadEnds().Count,2}, length from base={map.GetTreeHeight()}, length from boss={map.GetEccentricity(map.Boss)}, diameter={map.GetDiameter()}, boss-base dist={map.DistanceToBase(map.Boss)}{subtreeStr}");
        }

        private void RenderMap()
        {
            textBox1.Text = map.ToString();
            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                mapWriter.Draw(g, map, drawCritCheckBox.Checked, drawDeadEndsCheckBox.Checked);
            }
            pictureBox1.Refresh();
        }

        private void UpdateSize()
        {
            var size = sizeComboBox.SelectedIndex switch
            {
                0 => new Size(4, 4),
                1 => new Size(4, 8),
                11 => new Size(20, 20),
                var x when x >= 2 => new Size(x + 6, x + 6),
                _ => throw new NotImplementedException(),
            };

            map = new Map(size.Width, size.Height);
            pictureBox1.Width = size.Width * MapUtils.RoomSize + 24;
            pictureBox1.Height = size.Height * MapUtils.RoomSize + 24;
            pictureBox1.Image = new Bitmap(size.Width * MapUtils.RoomSize, size.Height * MapUtils.RoomSize);
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (randomSeedCheckbox.Checked)
                seedUpDown.Value = random.Next();

            Generate(randomRcCheckbox.Checked ? 0 : (int)rcUpDown.Value);
            rcUpDown.Value = map.Roomcount;
            RenderMap();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(pictureBox1.Image);
        }

        private void deleteDeadEndButton_Click(object sender, EventArgs e)
        {
            if (map != null)
            {
                mapGenerator.RemoveBonusDeadEnd(map);
                RenderMap();
            }
        }

        private void drawBox_CheckedChanged(object sender, EventArgs e)
        {
            RenderMap();
        }

        private void openStatsWindowButton_Click(object sender, EventArgs e)
        {
            new StatsForm().Show(this);
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Image Files|*.png|All Files|*.*" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var bmp = new Bitmap(ofd.FileName);
                var gameMap = new MapReader(Resources.ResourceManager).ReadMap(bmp, FloorSize.ByImageSize(bmp.Size));
                map = gameMap.Map;
                RenderMap();

                var subtreeStr = string.Empty;
                foreach (var dir in map.ChildrenDirs(map.Base))
                {
                    subtreeStr += $", {dir}={map.SubtreeSize(map.Base.Add(dir))}";
                }
                Logger.Log($"{(gameMap.IsComplete ? "[complete]   " : "[incomplete] ")}name={Path.GetFileName(ofd.FileName)}, rc={map.Roomcount}, dead end count={map.GetDeadEnds().Count,2}, tree height={map.GetTreeHeight()}, boss ecc={map.GetEccentricity(map.Boss)}, diameter={map.GetDiameter()}{subtreeStr}");
            }
        }

        private void sizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }
    }
}
