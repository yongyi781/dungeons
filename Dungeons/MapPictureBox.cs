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
        static readonly Pen AnnotationPen = new Pen(AnnotationColor, 1);
        static readonly Pen SelectionPen = new Pen(Color.DarkGreen, 1);
        static readonly Brush AnnotationBrush = new SolidBrush(AnnotationColor);
        static readonly Brush HomeBrush = Brushes.White;
        static readonly Brush DefaultRoomBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
        static readonly Pen GridLinePen = new Pen(Color.FromArgb(32, 255, 255, 255));
        static readonly Pen PathLinePen = new Pen(Color.FromArgb(80, 255, 255, 255));
        static readonly Font RoomCountFont = new Font("Georgia", 12);
        static readonly Brush RoomCountBrush = new SolidBrush(Color.FromArgb(200, 180, 180));

        private string[,] annotations = new string[8, 8];

        // For key annotations
        private string[] colors = { "c", "o", "y", "go", "gr", "b", "p", "s" };
        private Color[] colorValues = { Color.FromArgb(255, 178, 206), Color.Orange, Color.Yellow, Color.Gold, Color.Lime, Color.SkyBlue, Color.FromArgb(214, 178, 255), Color.Silver };

        private RoomType[,] roomTypes = new RoomType[MapSize, MapSize];
        private int[,] distances = new int[MapSize, MapSize];
        private Point[,] parents = new Point[MapSize, MapSize];

        public MapPictureBox()
        {
            ClearAnnotations();
            BuildMap();
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
                    Size = value.MapSize;
                }
            }
        }
        public Point SelectedLocation { get; set; }
        public int OpenedRoomCount { get; private set; }
        public int LeafCount { get; private set; }
        public Point HomeLocation { get; private set; }
        public Point BossLocation { get; private set; }
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
            if (!d.IsEmpty && FloorSize.IsValidMapCoords(Point.Add(SelectedLocation, d)))
            {
                SelectedLocation = Point.Add(SelectedLocation, d);
                Invalidate();
            }
        }

        public void ProcessKeyPress(KeyPressEventArgs e)
        {
            if (FloorSize.IsValidMapCoords(SelectedLocation))
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
            MarkedCriticalRooms.Clear();
            ComputeCriticalRooms();
            Invalidate();
        }

        public void BuildMap()
        {
            HomeLocation = MapUtils.NotFound;
            BossLocation = MapUtils.NotFound;
            OpenedRoomCount = 0;
            LeafCount = 0;
            for (int y = 0; y < FloorSize.NumRows; y++)
            {
                for (int x = 0; x < FloorSize.NumColumns; x++)
                {
                    var roomType = GetRoomType(new Point(x, y));
                    roomTypes[x, y] = roomType;
                    if (MapUtils.IsOpened(roomType))
                    {
                        ++OpenedRoomCount;
                        if (MapUtils.IsHome(roomType))
                            HomeLocation = new Point(x, y);
                        else if (MapUtils.IsBoss(roomType))
                            BossLocation = new Point(x, y);
                    }
                    if (MapUtils.IsLeaf(roomType) && !MapUtils.IsBoss(roomType) && !MapUtils.IsHome(roomType))
                        ++LeafCount;
                }
            }

            ComputeMapGraph();
            ComputeCriticalRooms();
        }

        public RoomType GetRoomType(Point p)
        {
            var pc = FloorSize.MapToClientCoords(p);
            return MapUtils.GetRoomType(Image as Bitmap, pc.X, pc.Y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var s = FloorSize.ClientToMapCoords(e.Location);
            if (FloorSize.IsValidMapCoords(s))
            {
                if (e.Button == MouseButtons.Left)
                    SelectedLocation = s;
                else if (e.Button == MouseButtons.Right && IsRoom(s))
                    ToggleMarkedCritical(s);
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        private void ComputeCriticalRooms()
        {
            CriticalRooms.Clear();
            CriticalRooms.Add(HomeLocation);
            var marked = MarkedCriticalRooms.Where(p => FloorSize.IsValidMapCoords(p) && IsRoom(p));
            if (FloorSize.IsValidMapCoords(BossLocation))
                marked = marked.Union(new Point[] { BossLocation });
            foreach (var room in marked)
            {
                for (var p = room; FloorSize.IsValidMapCoords(p) && p != HomeLocation; p = parents[p.X, p.Y])
                    CriticalRooms.Add(p);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //DrawSelectionRectangle(e);
            DrawGridLines(e);
            DrawPathLines(e);
            DrawAnnotations(e);

            // Draw distances
            if (DrawDistancesEnabled)
            {
                DrawDistances(e);
            }

            //DrawRoomCount(e);
        }

        private void DrawRoomCount(PaintEventArgs e)
        {
            // Center it
            var str = OpenedRoomCount.ToString();
            var strSize = e.Graphics.MeasureString(str, RoomCountFont);
            e.Graphics.DrawString(OpenedRoomCount.ToString(), RoomCountFont, RoomCountBrush, (Width - strSize.Width) / 2, 9);
        }

        private void DrawDistances(PaintEventArgs e)
        {
            for (int y = 0; y < FloorSize.NumRows; y++)
            {
                for (int x = 0; x < FloorSize.NumColumns; x++)
                {
                    var p = FloorSize.MapToClientCoords(new Point(x, y));
                    if (distances[x, y] > 0 && (MapUtils.IsLeaf(roomTypes[x, y]) || !MapUtils.IsOpened(roomTypes[x, y])))
                        e.Graphics.DrawString(distances[x, y].ToString(), DistanceAnnotationFont, Brushes.Pink, p.X + 3, p.Y + 3);
                }
            }
        }

        private void ToggleMarkedCritical(Point p)
        {
            if (MarkedCriticalRooms.Contains(p))
                MarkedCriticalRooms.Remove(p);
            else
                MarkedCriticalRooms.Add(p);
            ComputeCriticalRooms();
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
                        var p = FloorSize.MapToClientCoords(new Point(x, y));
                        e.Graphics.DrawString(ann, AnnotationFont, colorIndex == null ? AnnotationBrush : new SolidBrush(colorValues[colorIndex.Value]), p.X + 3, p.Y + 9);
                    }
                }
            }
        }

        private void DrawPathLines(PaintEventArgs e)
        {
            for (int y = 0; y < FloorSize.NumRows; y++)
            {
                for (int x = 0; x < FloorSize.NumColumns; x++)
                {
                    var p = new Point(x, y);
                    var center = FloorSize.MapToClientCoords(p);
                    center.Offset(16, 16);
                    var roomType = roomTypes[x, y];
                    DrawRoom(e, p);
                    if (IsRoom(p))
                    {

                        var pen = PathLinePen;
                        if ((roomType & RoomType.W) != 0)
                            e.Graphics.DrawLine(pen, center, new Point(center.X - 32, center.Y));
                        if ((roomType & RoomType.E) != 0 && !MapUtils.IsOpened(roomTypes[x + 1, y]))    // Don't double-draw lines
                            e.Graphics.DrawLine(pen, center, new Point(center.X + 32, center.Y));
                        if ((roomType & RoomType.S) != 0)
                            e.Graphics.DrawLine(pen, center, new Point(center.X, center.Y + 32));
                        if ((roomType & RoomType.N) != 0 && !MapUtils.IsOpened(roomTypes[x, y + 1]))
                            e.Graphics.DrawLine(pen, center, new Point(center.X, center.Y - 32));
                    }
                }
            }
        }

        private void DrawRoom(PaintEventArgs e, Point p)
        {
            var center = FloorSize.MapToClientCoords(p);
            center.Offset(16, 16);

            int size = 2;
            if (p == HomeLocation)
            {
                size = 3;
                e.Graphics.FillRectangle(HomeBrush, center.X - size, center.Y - size, 2 * size, 2 * size);
            }
            else if (p == BossLocation)
            {
                e.Graphics.FillRectangle(Brushes.Red, center.X - size, center.Y - size, 2 * size, 2 * size);
            }
            else if (MarkedCriticalRooms.Contains(p))
            {
                e.Graphics.FillRectangle(Brushes.Cyan, center.X - size, center.Y - size, 2 * size, 2 * size);
            }
            else if (CriticalRooms.Contains(p))
            {
                e.Graphics.FillRectangle(Brushes.Blue, center.X - size, center.Y - size, 2 * size, 2 * size);
            }
            else if (IsRoom(p))
            {
                size = 1;
                e.Graphics.FillRectangle(DefaultRoomBrush, center.X - size, center.Y - size, 2 * size, 2 * size);
            }
        }

        private void DrawSelectionRectangle(PaintEventArgs e)
        {
            if (FloorSize.IsValidMapCoords(SelectedLocation))
            {
                var p = FloorSize.MapToClientCoords(SelectedLocation);
                e.Graphics.DrawRectangle(SelectionPen, p.X, p.Y, 32, 32);
            }
        }

        private void DrawGridLines(PaintEventArgs e)
        {
            for (int y = -1; y <= FloorSize.NumRows - 1; y++)
            {
                var start = FloorSize.MapToClientCoords(new Point(0, y));
                var end = FloorSize.MapToClientCoords(new Point(FloorSize.NumColumns, y));
                e.Graphics.DrawLine(GridLinePen, start, end);
            }
            for (int x = 0; x <= FloorSize.NumColumns; x++)
            {
                var start = FloorSize.MapToClientCoords(new Point(x, -1));
                var end = FloorSize.MapToClientCoords(new Point(x, FloorSize.NumRows - 1));
                e.Graphics.DrawLine(GridLinePen, start, end);
            }
        }

        private bool IsRoom(Point p)
        {
            return FloorSize.IsValidMapCoords(p) && parents[p.X, p.Y] != MapUtils.NotFound;
        }

        private void ComputeMapGraph()
        {
            // Initialize
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    distances[i, j] = -1;
                    parents[i, j] = MapUtils.NotFound;
                }
            }

            var visited = new HashSet<Point>();

            void Visit(Point p, Point parent, int dist)
            {
                if (!FloorSize.IsValidMapCoords(p) || visited.Contains(p))
                    return;

                visited.Add(p);
                distances[p.X, p.Y] = dist;
                parents[p.X, p.Y] = parent;
                if ((roomTypes[p.X, p.Y] & RoomType.W) != 0)
                    Visit(new Point(p.X - 1, p.Y), p, dist + 1);
                if ((roomTypes[p.X, p.Y] & RoomType.E) != 0)
                    Visit(new Point(p.X + 1, p.Y), p, dist + 1);
                if ((roomTypes[p.X, p.Y] & RoomType.S) != 0)
                    Visit(new Point(p.X, p.Y - 1), p, dist + 1);
                if ((roomTypes[p.X, p.Y] & RoomType.N) != 0)
                    Visit(new Point(p.X, p.Y + 1), p, dist + 1);
            }

            Visit(HomeLocation, HomeLocation, 0);
        }
    }
}
