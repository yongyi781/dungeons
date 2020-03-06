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

        private Map map = new Map(WIDTH, HEIGHT);
        private MapGenerator mapGenerator;
        private readonly Random random = new Random();


        public Form1()
        {
            InitializeComponent();

            Logger.TextBox = logTextBox;
        }

        private async Task GenerateStepCallback()
        {
            RenderMap();
            await Task.Delay(20);
        }

        private async Task GenerateNormalBase()
        {
            mapGenerator = new MapGenerator((int)seedUpDown.Value, 19, 23);
            mapGenerator.GenerateBaseLocation(map);

            await mapGenerator.GeneratePrimVariant(map, slowCheckBox.Checked ? GenerateStepCallback : (Func<Task>)null);
            mapGenerator.AssignBossAndCritRooms(map);
            await mapGenerator.MakeGaps(map, (int)rcUpDown.Value, slowCheckBox.Checked ? GenerateStepCallback : (Func<Task>)null);

            Logger.Log($"seed={seedUpDown.Value}, rc={rcUpDown.Value}, crit count={map.CritRooms.Count + 1}, dead end count={map.GetDeadEnds().Count}");
        }

        private async Task GeneratePerpendicularBase()
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

            await mapGenerator.GeneratePrimVariant(map, slowCheckBox.Checked ? GenerateStepCallback : (Func<Task>)null);
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

        private int GenerateRoomcount()
        {
            if (random.Next(3) == 0)
            {
                // Generate 50-53 or 61-64
                return 50 + random.Next(4) + 11 * random.Next(2);
            }
            else
            {
                return 54 + random.Next(7);
            }
        }

        private void RenderMap()
        {
            textBox1.Text = map.ToString();
            var bmp = map.ToImage(drawCritCheckBox.Checked, drawDeadEndsCheckBox.Checked);
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = bmp;
        }

        private async void generateButton_Click(object sender, EventArgs e)
        {
            generateButton.Enabled = false;

            if (randomSeedCheckbox.Checked)
                seedUpDown.Value = random.Next();
            if (randomRcCheckbox.Checked)
                rcUpDown.Value = GenerateRoomcount();

            map.Clear();
            await GenerateNormalBase();
            RenderMap();

            generateButton.Enabled = true;
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
