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

        public Form1()
        {
            InitializeComponent();

            MapUtils.InitializeSignatures();
            mapPictureBox.FloorSize = FloorSize;
        }

        public DateTimeOffset TimerCheckpoint { get; private set; } = DateTimeOffset.MinValue;

        public FloorSize FloorSize
        {
            get => mapPictureBox.FloorSize;
            set => mapPictureBox.FloorSize = value;
        }

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
        }

        private Point FindMap()
        {
            var size = SystemInformation.VirtualScreen.Size;
            var bmp = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, 0, 0, size);
            }

            // Search for map marker
            var desktopBounds = DesktopBounds;
            desktopBounds.Offset(-SystemInformation.VirtualScreen.X, -SystemInformation.VirtualScreen.Y);
            var match = UnsafeBitmap.FindMatch(bmp, mapMarker, p => !desktopBounds.Contains(p));
            match.Offset(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y);
            return match;
        }

        private void UpdateMap()
        {
            foreach (var floorSize in new FloorSize[] { FloorSize, FloorSize.Large, FloorSize.Medium, FloorSize.Small })
            {
                FloorSize = floorSize;
                var bmp = new Bitmap(FloorSize.MapSize.Width, FloorSize.MapSize.Height);

                if (Properties.Settings.Default.MapLocation != MapUtils.NotFound)
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        try
                        {
                            g.CopyFromScreen(Properties.Settings.Default.MapLocation.X - FloorSize.MarkerLocation.X, Properties.Settings.Default.MapLocation.Y - FloorSize.MarkerLocation.Y, 0, 0, bmp.Size);
                        }
                        catch (Win32Exception)
                        {
                            return;
                        }
                    }
                    if (UnsafeBitmap.IsMatch(bmp, mapMarker, FloorSize.MarkerLocation.X, FloorSize.MarkerLocation.Y))
                    {
                        timer.Start();
                        mapPictureBox.Image = bmp;
                        saveMapButton.Enabled = true;
                        var prevHomeLocation = mapPictureBox.HomeLocation;
                        mapPictureBox.BuildMap();
                        UpdateDataLabel();

                        // Reset when home changes or on first map load
                        if (TimerCheckpoint == DateTimeOffset.MinValue || (mapPictureBox.HomeLocation != MapUtils.NotFound
                            && prevHomeLocation != MapUtils.NotFound
                            && mapPictureBox.HomeLocation != prevHomeLocation))
                        {
                            TimerCheckpoint = DateTimeOffset.Now.AddSeconds(-3);
                        }
                        if (mapPictureBox.OpenedRoomCount > 0)
                            break;  // Found a floor size that aligns correctly with the rooms, this must be the right one.
                    }
                    else
                    {
                        // No map marker was even found, skip trying other floor sizes.
                        break;
                    }
                }
            }
        }

        private void UpdateDataLabel()
        {
            var roomsPerMinStr = (mapPictureBox.OpenedRoomCount / GetElapsedTime().TotalMinutes).ToString("0.0");
            dataLabel.Text = $"{mapPictureBox.OpenedRoomCount} rooms opened | {roomsPerMinStr} rooms/min | {mapPictureBox.CriticalRooms.Count} critical";
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
                Properties.Settings.Default.MapLocation = mapLocation;
                Properties.Settings.Default.Save();
                UpdateMap();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateMap();

            if (TimerCheckpoint != DateTimeOffset.MinValue)
            {
                UpdateTimerLabel();
                UpdateDataLabel();
            }
        }

        private void UpdateTimerLabel()
        {
            var elapsed = GetElapsedTime();
            timerLabel.Text = elapsed.ToString(elapsed.Hours > 0 ? "h\\:mm\\:ss" : "m':'ss");
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
            // Close if on close button

            UpdateDataLabel();
        }

        private void clearAnnotationsButton_Click(object sender, EventArgs e)
        {
            mapPictureBox.ClearAnnotations();
        }

        private void topMostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = topMostCheckBox.Checked;
        }

        private void resetTimerButton_Click(object sender, EventArgs e)
        {
            TimerCheckpoint = DateTimeOffset.Now;
        }

        private void plusOneOrTenButton_Click(object sender, EventArgs e)
        {
            if (TimerCheckpoint != DateTimeOffset.MinValue)
            {
                if (sender == plusOneButton)
                    TimerCheckpoint = TimerCheckpoint.AddSeconds(-1);
                else
                    TimerCheckpoint = TimerCheckpoint.AddSeconds(-10);
            }
            UpdateTimerLabel();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
