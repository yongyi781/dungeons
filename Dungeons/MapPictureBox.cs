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
        static readonly Pen AnnotationPen = new Pen(AnnotationColor, 2);
        static readonly Brush AnnotationBrush = new SolidBrush(AnnotationColor);
        private string[,] annotations = new string[8, 8];

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

            if (MapUtils.IsValidMapCoords(SelectedLocation))
            {
                var p = MapUtils.MapToClientCoords(SelectedLocation);
                e.Graphics.DrawRectangle(AnnotationPen, p.X, p.Y, 32, 32);
            }

            for (int y = 0; y < annotations.GetLength(0); y++)
            {
                for (int x = 0; x < annotations.GetLength(1); x++)
                {
                    var ann = annotations[y, x];
                    if (!string.IsNullOrWhiteSpace(ann))
                    {
                        var p = MapUtils.MapToClientCoords(new Point(x + 1, y + 1));
                        e.Graphics.DrawString(ann, AnnotationFont, AnnotationBrush, p.X + 3, p.Y + 9);
                    }
                }
            }
        }
    }
}
