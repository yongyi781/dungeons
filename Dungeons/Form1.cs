using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dungeons
{
    public partial class Form1 : Form
    {
        const int MapOffsetX = 2;
        const int MapOffsetY = 2;
        const int MapWidth = 318;
        const int MapHeight = 310;
        const int MapGridOffsetX = 29;
        const int MapGridOffsetY = 27;

        const int WM_NCLBUTTONDOWN = 0xA1;
        const int WM_NCHITTEST = 0x84;
        const int HT_CAPTION = 0x2;
        private const int WM_LBUTTONDBLCLK = 0x00A3;

        private static readonly Keys[] KeysToEat =
        {
            Keys.Enter,
            Keys.Space,
            Keys.Up,
            Keys.Left,
            Keys.Right,
            Keys.Down
        };

        private Bitmap mapMarker = Properties.Resources.MapMarker;
        private Point mapLocation = new Point(2703, 370);
        private MapForm mapForm;

        public Form1()
        {
            InitializeComponent();

            MapUtils.InitializeSignatures();
            //mapForm = new MapForm(this);
        }

        public DateTimeOffset TimerCheckpoint { get; private set; } = DateTimeOffset.MinValue;
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (KeysToEat.Contains(keyData))
            {
                mapPictureBox.ProcessKeyDown(keyData);
                UpdateDataLabel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            mapPictureBox.ProcessKeyPress(e);
            base.OnKeyPress(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Start on top right, lol
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, 0);
            //mapForm.Show();
        }
        
        private Point FindMap()
        {
            var size = SystemInformation.VirtualScreen.Size;
            var bmp = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, size);
            }

            // Search for map marker
            return UnsafeBitmap.FindMatch(bmp, mapMarker, p => !DesktopBounds.Contains(p));
        }

        private void UpdateMap()
        {
            var bmp = new Bitmap(MapWidth, MapHeight);

            if (mapLocation != MapUtils.NotFound)
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    try
                    {
                        g.CopyFromScreen(mapLocation.X - MapOffsetX, mapLocation.Y - MapOffsetY, 0, 0, bmp.Size);
                    }
                    catch (Win32Exception)
                    {
                        return;
                    }
                }
                if (UnsafeBitmap.IsMatch(bmp, mapMarker, MapOffsetX, MapOffsetY))
                {
                    timer.Start();
                    mapPictureBox.Image = bmp;
                    saveMapButton.Enabled = true;
                    var prevHomeLocation = mapPictureBox.HomeLocation;
                    mapPictureBox.BuildMap();
                    if (mapForm != null)
                        mapForm.UpdateMapImage(bmp);
                    UpdateDataLabel();

                    // Reset when home changes or on first map load
                    if (TimerCheckpoint == DateTimeOffset.MinValue || (mapPictureBox.HomeLocation != MapUtils.NotFound
                        && prevHomeLocation != MapUtils.NotFound
                        && mapPictureBox.HomeLocation != prevHomeLocation))
                    {
                        TimerCheckpoint = DateTimeOffset.Now.AddSeconds(-3);
                    }
                }
            }
        }

        private void UpdateDataLabel()
        {
            var roomsPerMinStr = ((mapPictureBox.OpenedRoomCount - 1) / GetElapsedTime().TotalMinutes).ToString("0.0");
            dataLabel.Text = $"{mapPictureBox.OpenedRoomCount} rooms opened | {roomsPerMinStr} rooms/min | {mapPictureBox.CriticalRooms.Count} critical rooms";
        }

        private TimeSpan GetElapsedTime()
        {
            return DateTimeOffset.Now - TimerCheckpoint;
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            var mapLocation = FindMap();
            if (mapLocation != MapUtils.NotFound)
            {
                this.mapLocation = mapLocation;
                UpdateMap();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateMap();

            if (TimerCheckpoint != DateTimeOffset.MinValue)
            {
                var elapsed = GetElapsedTime();
                timerLabel.Text = elapsed.ToString(elapsed.Hours > 0 ? "h\\:mm\\:ss" : "m':'ss");
                UpdateDataLabel();
            }
        }

        private void saveMapButton_Click(object sender, EventArgs e)
        {
            var fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            mapPictureBox.Image.Save(Path.Combine(Properties.Settings.Default.MapSaveLocation, $"map_{fileName}.png"));

            savedLabel.Visible = true;
            saveLabelHideTimer.Start();
        }

        private void saveLabelHideTimer_Tick(object sender, EventArgs e)
        {
            savedLabel.Visible = false;
            saveLabelHideTimer.Stop();
        }

        private void mapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateDataLabel();
        }

        private void clearAnnotationsButton_Click(object sender, EventArgs e)
        {
            mapPictureBox.ClearAnnotations();
        }

        private void distancesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mapPictureBox.DrawDistancesEnabled = distancesCheckBox.Checked;
            mapPictureBox.Invalidate();
        }

        private void resetTimerButton_Click(object sender, EventArgs e)
        {
            TimerCheckpoint = DateTimeOffset.Now;
        }

        private void plusOneOrTenButton_Click(object sender, EventArgs e)
        {
            if (sender == plusOneButton)
                TimerCheckpoint = TimerCheckpoint.AddSeconds(-1);
            else
                TimerCheckpoint = TimerCheckpoint.AddSeconds(-10);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
