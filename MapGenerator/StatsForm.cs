using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class StatsForm : Form
    {
        MapGenerator generator = new MapGenerator(0, 19, 23);

        public StatsForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
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
                generator.GeneratePrimVariant(map);
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
                generator.GeneratePrimVariant(map);
                generator.AssignBossAndCritRooms(map);
                generator.MakeGaps(map, roomcount);

                if (HasColumnOrRowGap(map))
                    ++count;
            }
            resultsTextBox.AppendText($"rc={roomcount}, trials={trials}, p={(double)count / trials}{Environment.NewLine}");
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
                default:
                    break;
            }
        }
    }
}
