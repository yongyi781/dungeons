using Dungeons.Common;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class Form1 : Form
    {
        private const int WIDTH = 8;
        private const int HEIGHT = 8;

        private Map map = new Map(WIDTH, HEIGHT);
        private readonly MapWriter mapWriter = new MapWriter(Resources.ResourceManager);
        private readonly Random random = new Random();
        private MapGenerator mapGenerator;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(WIDTH * MapUtils.RoomSize, HEIGHT * MapUtils.RoomSize);
            comboBox1.SelectedIndex = 0;

            Logger.TextBox = logTextBox;
        }

        private void GenerateNormalBase()
        {
            mapGenerator = new MapGenerator((int)seedUpDown.Value, FloorSize.ByDimensions(WIDTH, HEIGHT));
            var bossFaceDir = mapGenerator.MakeBossAndStartingGaps(map, (int)rcUpDown.Value);
            map.Base = map.Boss;

            switch (comboBox1.SelectedIndex)
            {
                case 1:
                    mapGenerator.GenerateAldousBroder(map, bossFaceDir);
                    break;
                default:
                    mapGenerator.GeneratePrimDeadEndBase(map, bossFaceDir);
                    break;
            }

            // Rebase
            mapGenerator.AssignCritRooms(map);
            mapGenerator.Rebase(map);

            var subtreeStr = string.Empty;
            foreach (var dir in map.ChildrenDirs(map.Base))
            {
                subtreeStr += $", {dir}={map.SubtreeSize(map.Base.Add(dir))}";
            }

            Logger.Log($"seed={seedUpDown.Value,10}, rc={map.Roomcount}, crit count={map.GetCritRooms().Count}, dead end count={map.GetDeadEnds().Count,2}, tree height={map.GetTreeHeight()}, boss ecc={map.GetEccentricity(map.Boss)}, diameter={map.GetDiameter()}, boss-base dist={map.DistanceToBase(map.Boss)}{subtreeStr}");
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

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (randomSeedCheckbox.Checked)
                seedUpDown.Value = random.Next();
            if (randomRcCheckbox.Checked)
                rcUpDown.Value = random.RandomRoomcount(FloorSize.ByDimensions(WIDTH, HEIGHT));

            map.Clear();
            GenerateNormalBase();
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
    }
}
