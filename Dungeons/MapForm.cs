using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeons
{
    public partial class MapForm : Form
    {
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        static readonly Font RoomCountFont = new Font("Georgia", 12);
        static readonly Brush RoomCountBrush = new SolidBrush(Color.FromArgb(50, 30, 30));

        private Form1 parent;

        public MapForm(Form1 parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        public Map Map { get; set; } = new Map();
        
        public void UpdateMapImage(Bitmap image)
        {
            Map.Image = image;
            Invalidate();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.InterpolationMode = InterpolationMode.High;

            if (Map.Image != null)
            {
                e.Graphics.DrawImage(Map.Image, 0, 0, Width, Height);
            }

            DrawRoomCount(e);
            DrawRoomsPerMinute(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && (ModifierKeys & Keys.Alt) != 0)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            UpdateCursor();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            UpdateCursor();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            UpdateCursor();
        }

        private void UpdateCursor()
        {
            Cursor = (ModifierKeys & Keys.Alt) != 0 ? Cursors.SizeAll : Cursors.Default;
        }

        private void DrawRoomCount(PaintEventArgs e)
        {
            // Center it
            var str = Map.OpenedRoomCount.ToString();
            var strSize = e.Graphics.MeasureString(str, RoomCountFont);
            e.Graphics.DrawString(str, RoomCountFont, RoomCountBrush, (Width - strSize.Width) / 2, 9);
        }

        private void DrawRoomsPerMinute(PaintEventArgs e)
        {
            // Center it
            var roomsPerMin = Map.GetRoomsPerMinute(DateTimeOffset.Now - parent.TimerCheckpoint);
            var str = roomsPerMin.ToString("0.0");
            var strSize = e.Graphics.MeasureString(str, RoomCountFont);
            e.Graphics.DrawString(str, RoomCountFont, RoomCountBrush, (Width - strSize.Width) / 2, Height - strSize.Height - 9);
        }
    }
}
