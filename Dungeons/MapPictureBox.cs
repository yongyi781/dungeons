using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeons
{
    public class MapPictureBox : PictureBox
    {
        const int MaxAnnotationLength = 4;
        const int MapSize = 8;

        static readonly Font AnnotationFont = new Font("Consolas", 8);
        static readonly Font DistanceAnnotationFont = new Font("Arial", 7);
        static readonly Color AnnotationColor = Color.FromArgb(240, 240, 240);
        //static readonly Pen AnnotationPen = new Pen(AnnotationColor, 1);
        static readonly Pen SelectionPen = new Pen(Color.DarkGreen, 1);
        static readonly Brush AnnotationBrush = new SolidBrush(AnnotationColor);
        static readonly Brush HomeBrush = Brushes.White;
        static readonly Brush DefaultRoomBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
        static readonly Pen GridLinePen = new Pen(Color.FromArgb(32, 255, 255, 255));
        static readonly Pen PathLinePen = new Pen(Color.FromArgb(80, 255, 255, 255));
        //static readonly Font RoomCountFont = new Font("Georgia", 12);
        //static readonly Brush RoomCountBrush = new SolidBrush(Color.FromArgb(200, 180, 180));

        private readonly string[,] annotations = new string[8, 8];

        // For key annotations
        private readonly string[] colors = { "c", "o", "y", "go", "gr", "b", "p", "s" };
        private readonly Color[] colorValues = { Color.FromArgb(255, 178, 206), Color.Orange, Color.Yellow, Color.Gold, Color.Lime, Color.SkyBlue, Color.FromArgb(214, 178, 255), Color.Silver };

        private readonly MapReader mapReader;

        public MapPictureBox()
        {
            mapReader = new MapReader(Properties.Resources.ResourceManager);

            ClearAnnotations();
            ReadMap();
        }

        private FloorSize floorSize = FloorSize.Large;
        public FloorSize FloorSize
        {
            get => floorSize;
            set
            {
                if (floorSize != value)
                {
                    floorSize = value;
                    Invalidate();
                }
            }
        }

        public GameMap GameMap { get; private set; } = new GameMap(new RoomType[8, 8]);
        public Point SelectedLocation { get; set; }
        public HashSet<Point> MarkedCriticalRooms { get; private set; } = new HashSet<Point>();
        public HashSet<Point> CriticalRooms { get; private set; } = new HashSet<Point>();
        public bool DrawDistancesEnabled { get; set; }

        public void ProcessKeyDown(Keys keyData)
        {
            var d = Size.Empty;
            switch (keyData)
            {
                case Keys.Left:
                    d = new Size(-1, 0);
                    break;
                case Keys.Up:
                    d = new Size(0, 1);
                    break;
                case Keys.Right:
                    d = new Size(1, 0);
                    break;
                case Keys.Down:
                    d = new Size(0, -1);
                    break;
                default:
                    break;
            }
            if (!d.IsEmpty && FloorSize.IsInRange(Point.Add(SelectedLocation, d)))
            {
                SelectedLocation = Point.Add(SelectedLocation, d);
                Invalidate();
            }
        }

        public void ProcessKeyPress(KeyPressEventArgs e)
        {
            if (FloorSize.IsInRange(SelectedLocation))
            {
                var i = SelectedLocation.Y;
                var j = SelectedLocation.X;

                if (e.KeyChar == 27)    // Esc
                {
                    annotations[i, j] = string.Empty;
                }
                else if (e.KeyChar == '\b')
                {
                    if (!string.IsNullOrEmpty(annotations[i, j]))
                        annotations[i, j] = annotations[i, j].Substring(0, annotations[i, j].Length - 1);
                }
                else if (!char.IsControl(e.KeyChar) && (annotations[i, j] == null || annotations[i, j].Length < MaxAnnotationLength))
                {
                    annotations[i, j] += e.KeyChar;
                }
                Invalidate();
            }

            base.OnKeyPress(e);
        }

        public void ClearAnnotations()
        {
            for (int y = 0; y < annotations.GetLength(0); y++)
                for (int x = 0; x < annotations.GetLength(1); x++)
                    annotations[y, x] = string.Empty;
            Invalidate();
        }

        public void ReadMap()
        {
            GameMap = mapReader.ReadMap((Bitmap)Image, floorSize);
        }

        public RoomType ReadRoom(Point p)
        {
            var pc = FloorSize.MapToClientCoords(p, Image.Size);
            return mapReader.ReadRoom(Image as Bitmap, pc.X, pc.Y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Image != null)
            {
                var s = FloorSize.ClientToMapCoords(e.Location, Image.Size);
                if (FloorSize.IsInRange(s))
                {
                    if (e.Button == MouseButtons.Left)
                        SelectedLocation = s;
                    else if (e.Button == MouseButtons.Right && GameMap.IsRoom(s))
                        ToggleMarkedCritical(s);
                    Invalidate();
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            DrawAnnotations(e);
        }

        private void ToggleMarkedCritical(Point p)
        {
            if (MarkedCriticalRooms.Contains(p))
                MarkedCriticalRooms.Remove(p);
            else
                MarkedCriticalRooms.Add(p);
            //ComputeCriticalRooms();
        }

        private void DrawAnnotations(PaintEventArgs e)
        {
            for (int y = 0; y < FloorSize.NumRows; y++)
            {
                for (int x = 0; x < FloorSize.NumColumns; x++)
                {
                    var ann = annotations[y, x];
                    var colorIndex = colors.Select((c, i) => new { c, i }).FirstOrDefault(c => ann.StartsWith(c.c))?.i;
                    if (ann.StartsWith("bo") || ann == "c" || ann.StartsWith("cri"))
                        colorIndex = null;
                    if (!string.IsNullOrWhiteSpace(ann))
                    {
                        var p = FloorSize.MapToClientCoords(new Point(x, y), Image.Size);
                        e.Graphics.DrawString(ann, AnnotationFont, colorIndex == null ? AnnotationBrush : new SolidBrush(colorValues[colorIndex.Value]), p.X + 3, p.Y + 9);
                    }
                }
            }
        }
    }
}
