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

        static readonly Font AnnotationFont = new Font("Consolas", 8);
        static readonly Color AnnotationColor = Color.FromArgb(240, 240, 240);
        static readonly Pen AnnotationPen = new Pen(AnnotationColor, 1);
        static readonly Brush AnnotationBrush = new SolidBrush(AnnotationColor);
        static readonly Pen gridLinePen = new Pen(Brushes.Bisque)
        {
            DashCap = System.Drawing.Drawing2D.DashCap.Round,
            DashPattern = new float[] { 1.0f, 1.0f }
        };

        private string[,] annotations = new string[8, 8];

        private string[] colors = { "c", "o", "y", "go", "gr", "b", "p", "s" };
        private Color[] colorValues = { Color.FromArgb(255, 178, 206), Color.Orange, Color.Yellow, Color.Gold, Color.Lime, Color.SkyBlue, Color.FromArgb(214, 178, 255), Color.Silver };

        public MapPictureBox()
        {
            ClearAnnotations();
        }

        public Point SelectedLocation { get; set; }

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
            if (!d.IsEmpty && MapUtils.IsValidMapCoords(Point.Add(SelectedLocation, d)))
            {
                SelectedLocation = Point.Add(SelectedLocation, d);
                Invalidate();
            }
        }

        public void ProcessKeyPress(KeyPressEventArgs e)
        {
            var i = SelectedLocation.Y - 1;
            var j = SelectedLocation.X - 1;


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

            base.OnKeyPress(e);
        }

        public void ClearAnnotations()
        {
            for (int y = 0; y < annotations.GetLength(0); y++)
            {
                for (int x = 0; x < annotations.GetLength(1); x++)
                {
                    annotations[y, x] = string.Empty;
                }
            }
            Invalidate();
        }

        public RoomType GetRoomType(Point p)
        {
            var pc = MapUtils.MapToClientCoords(p);
            return MapUtils.GetRoomType(Image as Bitmap, pc.X, pc.Y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var s = MapUtils.ClientToMapCoords(e.Location);
            if (MapUtils.IsValidMapCoords(s))
            {
                SelectedLocation = s;
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //DrawGridLines(e);

            // Draw selection rectangle
            if (MapUtils.IsValidMapCoords(SelectedLocation))
            {
                var p = MapUtils.MapToClientCoords(SelectedLocation);
                e.Graphics.DrawRectangle(AnnotationPen, p.X, p.Y, 32, 32);
            }

            // Draw path lines
            for (int y = 1; y <= 8; y++)
            {
                for (int x = 1; x <= 8; x++)
                {
                    var p = new Point(x, y);
                    var center = MapUtils.MapToClientCoords(p);
                    center.Offset(16, 16);
                    var roomType = GetRoomType(p);
                    if (MapUtils.IsOpened(roomType))
                    {
                        const int size = 1;
                        e.Graphics.FillRectangle(Brushes.Bisque, center.X - size, center.Y - size, 2 * size, 2 * size);

                        var pen = gridLinePen;
                        if ((roomType & RoomType.W) != 0)
                            e.Graphics.DrawLine(pen, center, new Point(center.X - 32, center.Y));
                        if ((roomType & RoomType.E) != 0)
                            e.Graphics.DrawLine(pen, center, new Point(center.X + 32, center.Y));
                        if ((roomType & RoomType.S) != 0)
                            e.Graphics.DrawLine(pen, center, new Point(center.X, center.Y + 32));
                        if ((roomType & RoomType.N) != 0)
                            e.Graphics.DrawLine(pen, center, new Point(center.X, center.Y - 32));
                    }
                }
            }

            // Draw annotations
            for (int y = 0; y < annotations.GetLength(0); y++)
            {
                for (int x = 0; x < annotations.GetLength(1); x++)
                {
                    var ann = annotations[y, x];
                    var colorIndex = colors.Select((c, i) => new { c, i }).FirstOrDefault(c => ann.StartsWith(c.c))?.i;
                    if (ann.StartsWith("bo") || ann == "c" || ann.StartsWith("cri"))
                        colorIndex = null;
                    if (!string.IsNullOrWhiteSpace(ann))
                    {
                        var p = MapUtils.MapToClientCoords(new Point(x + 1, y + 1));
                        e.Graphics.DrawString(ann, AnnotationFont, colorIndex == null ? AnnotationBrush : new SolidBrush(colorValues[colorIndex.Value]), p.X + 3, p.Y + 9);
                    }
                }
            }
        }

        private void DrawGridLines(PaintEventArgs e)
        {
            for (int y = 0; y <= 8; y++)
            {
                var start = MapUtils.MapToClientCoords(new Point(1, y));
                var end = MapUtils.MapToClientCoords(new Point(9, y));
                e.Graphics.DrawLine(gridLinePen, start, end);
            }
            for (int x = 1; x <= 9; x++)
            {
                var start = MapUtils.MapToClientCoords(new Point(x, 0));
                var end = MapUtils.MapToClientCoords(new Point(x, 8));
                e.Graphics.DrawLine(gridLinePen, start, end);
            }
        }
    }
}
