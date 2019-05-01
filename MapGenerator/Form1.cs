using Dungeons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class Form1 : Form
    {
        private static readonly Point Invalid = new Point(-1, -1);
        private static readonly Size[] offsets = { new Size(0, 1), new Size(0, -1), new Size(1, 0), new Size(-1, 0) };
        private static readonly Dictionary<Size, RoomType> offsetRoomTypeDict = new Dictionary<Size, RoomType>
        {
            [new Size(0, 1)] = RoomType.N,
            [new Size(0, -1)] = RoomType.S,
            [new Size(1, 0)] = RoomType.E,
            [new Size(-1, 0)] = RoomType.W,
        };

        private Brush HomeBrush;

        public Form1()
        {
            InitializeComponent();

            HomeBrush = Brushes.White;
        }

        private bool IsInRange(Point p) => p.X >= 0 && p.X < 8 && p.Y >= 0 && p.Y < 8;

        private string GetRoomTypeResourceString(RoomType type)
        {
            if (type == RoomType.NotOpened)
                return "NotOpened";

            var str = "Room";
            if ((type & RoomType.E) != 0)
                str += "E";
            if ((type & RoomType.N) != 0)
                str += "N";
            if ((type & RoomType.S) != 0)
                str += "S";
            if ((type & RoomType.W) != 0)
                str += "W";
            return str;
        }

        private Bitmap BuildMap(Point[,] parents, Point home)
        {
            var bmp = new Bitmap(32 * 8, 32 * 8);
            var roomTypes = new RoomType[8, 8];
            RoomType[] dirs = { RoomType.N, RoomType.S, RoomType.E, RoomType.W };
            using (var g = Graphics.FromImage(bmp))
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        var offset = new Size(parents[x, y].X - x, parents[x, y].Y - y);
                        var roomType = RoomType.NotOpened;
                        if (offsetRoomTypeDict.ContainsKey(offset))
                            roomType |= offsetRoomTypeDict[offset];
                        var p = new Point(x, y);
                        for (int i = 0; i < 4; i++)
                        {
                            var p2 = p + offsets[i];
                            if (IsInRange(p2) && parents[p2.X, p2.Y] == p)
                                roomType |= dirs[i];
                        }
                        var roomBmp = (Bitmap)Properties.Resources.ResourceManager.GetObject(GetRoomTypeResourceString(roomType));
                        g.DrawImage(roomBmp, x * 32, (7 - y) * 32, 32, 32);
                    }
                }
                g.FillRectangle(HomeBrush, 32 * home.X + 13, 32 * (7 - home.Y) + 13, 6, 6);
            }
            return bmp;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            var parents = new Point[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    parents[x, y] = Invalid;
                }
            }
            var random = new Random();
            var agenda = new Queue<Point>();
            var home = new Point(random.Next(8), random.Next(8));
            agenda.Enqueue(home);
            while (agenda.Count > 0)
            {
                var curr = agenda.Dequeue();
                var neighbors = (from d in offsets
                                 let p = Point.Add(curr, d)
                                 where IsInRange(p)
                                 select p).ToArray();
                neighbors.Shuffle();
                foreach (var n in neighbors)
                {
                    if (parents[n.X, n.Y] == Invalid && random.NextDouble() < (double)probabilityUpDown.Value)
                    {
                        parents[n.X, n.Y] = curr;
                        agenda.Enqueue(n);
                    }
                }
            }

            var bmp = BuildMap(parents, home);
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = bmp;
        }
    }

    [Flags]
    enum Direction { None, N = 1, S = 2, E = 4, W = 8 }
}
