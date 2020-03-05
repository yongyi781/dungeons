using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            Logger.TextBox = logTextBox;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            MapGenerator.SetSeed((int)seedUpDown.Value);
            map = MapGenerator.Generate4((int)rcUpDown.Value);

            RenderMap();
            Logger.Log($"Last seed={seedUpDown.Value}, rc={rcUpDown.Value}, critcount={map.CritRooms.Count}");
            if (randomSeedCheckbox.Checked)
                seedUpDown.Value = random.Next();
            if (randomRcCheckbox.Checked)
                rcUpDown.Value = GenerateRoomcount();
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
            var bmp = map.ToImage(false, true);
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
                MapGenerator.RemoveDeadEnd(map);
                RenderMap();
            }
        }
    }
}
