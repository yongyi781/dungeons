using Dungeons.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class Form1 : Form
    {
        private const int WIDTH = 8;
        private const int HEIGHT = 8;

        private readonly Map map = new Map(WIDTH, HEIGHT);
        private readonly MapWriter mapWriter = new MapWriter(Resources.ResourceManager);
        private readonly Random random = new Random();
        private MapGenerator mapGenerator;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(WIDTH * MapUtils.RoomSize, HEIGHT * MapUtils.RoomSize);

            Logger.TextBox = logTextBox;
        }

        private void GenerateNormalBase()
        {
            mapGenerator = new MapGenerator((int)seedUpDown.Value, 19, 23);
            mapGenerator.GenerateBaseLocation(map);

            mapGenerator.GeneratePrimVariant(map);
            mapGenerator.AssignBossAndCritRooms(map);
            mapGenerator.MakeGaps(map, (int)rcUpDown.Value);

            Logger.Log($"seed={seedUpDown.Value}, rc={rcUpDown.Value}, crit count={map.GetCritRooms().Count}, dead end count={map.GetDeadEnds().Count}, east size={map.SubtreeSize(map.Base.Add(Direction.E))}");
        }

        private void GeneratePerpendicularBase()
        {
            mapGenerator = new MapGenerator((int)seedUpDown.Value, 19, 23);

            // Perpendicular base
            map.Base = new Point(1, 0);
            var wp = map.Base.Add(Direction.W);
            var np = map.Base.Add(Direction.N);
            var ep = map.Base.Add(Direction.E);

            map[wp] = Direction.E;
            map[np] = Direction.S;
            map[ep] = Direction.W;

            mapGenerator.GeneratePrimVariant(map);
            //mapGenerator.AssignBossAndCritRooms(map);
            //await mapGenerator.MakeGaps(map, (int)rcUpDown.Value);

            if (map.ChildrenDirs(map.Base).Count() == 3)
            {
                var ws = map.SubtreeSize(wp);
                var ns = map.SubtreeSize(np);
                var es = map.SubtreeSize(ep);
                var max = Math.Max(ws, Math.Max(ns, es));

                //Logger.Log($"seed={seedUpDown.Value}, rc={rcUpDown.Value}, crit count={map.CritRooms.Count}, dead end count={map.GetDeadEnds().Count}, w={ws}, n={ns}, e={es}");
                Logger.Log($"{(ws==max?1:0)}\t{(ns == max ? 1 : 0)}\t{(es == max ? 1 : 0)}");
            }
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
                rcUpDown.Value = random.RandomRoomcount(FloorSize.Large);

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
    }
}
