using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dungeons
{
    public partial class Form1 : Form
    {
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        private Point lastHomeLocation = MapUtils.NotFound;
        private int lastRoomCount = 0;
        private readonly WinterfaceWindow winterfaceWindow;

        private static readonly Keys[] KeysToEat =
        {
            Keys.Enter,
            Keys.Space,
            Keys.Up,
            Keys.Left,
            Keys.Right,
            Keys.Down
        };

        private Bitmap mapMarker;

        public Form1()
        {
            InitializeComponent();

            MapUtils.InitializeRoomsAndSignatures();
            FontType.InitializeFonts();
            mapPictureBox.FloorSize = FloorSize;
            winterfaceWindow = new WinterfaceWindow(this);
            mapMarker = Properties.Resources.MapMarker;
        }

        public DateTimeOffset FloorStartTime { get; private set; } = DateTimeOffset.MinValue;
        public int Roomcount => mapPictureBox.OpenedRoomCount;
        public int LeafCount => mapPictureBox.LeafCount;

        public FloorSize FloorSize
        {
            get => mapPictureBox.FloorSize;
            set => mapPictureBox.FloorSize = value;
        }

        public void SaveMap()
        {
            if (mapPictureBox.Image != null && Directory.Exists(Properties.Settings.Default.MapSaveLocation))
            {
                var fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                mapPictureBox.Image.Save(Path.Combine(Properties.Settings.Default.MapSaveLocation, $"map_{fileName}.png"));

                savedLabel.Visible = true;
                saveLabelHideTimer.Start();
            }
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
                NativeMethods.SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
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
            winterfaceWindow.Show();
        }

        private Point FindMap()
        {
            var size = SystemInformation.VirtualScreen.Size;
            using (var bmp = new Bitmap(size.Width, size.Height))
            {
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
        }

        private void UpdateMap()
        {
            foreach (var floorSize in new FloorSize[] { FloorSize, FloorSize.Large, FloorSize.Medium, FloorSize.Small })
            {
                if (Properties.Settings.Default.MapLocation != MapUtils.NotFound)
                {
                    var bmp = new Bitmap(floorSize.MapSize.Width, floorSize.MapSize.Height);
                    using (var g = Graphics.FromImage(bmp))
                    {
                        try
                        {
                            g.CopyFromScreen(Properties.Settings.Default.MapLocation.X - floorSize.MarkerLocation.X, Properties.Settings.Default.MapLocation.Y - floorSize.MarkerLocation.Y, 0, 0, bmp.Size);
                        }
                        catch (Win32Exception)
                        {
                            return;
                        }
                    }
                    if (UnsafeBitmap.IsMatch(bmp, mapMarker, floorSize.MarkerLocation.X, floorSize.MarkerLocation.Y))
                    {
                        FloorSize = floorSize;
                        timer.Start();
                        if (mapPictureBox.Image != null)
                            mapPictureBox.Image.Dispose();
                        mapPictureBox.Image = bmp;
                        saveMapButton.Enabled = true;
                        mapPictureBox.BuildMap();
                        UpdateDataLabel();

                        // Reset when home changes or on first map load
                        if (FloorStartTime == DateTimeOffset.MinValue || (mapPictureBox.HomeLocation != MapUtils.NotFound
                            && lastHomeLocation != MapUtils.NotFound
                            && mapPictureBox.HomeLocation != lastHomeLocation)
                            || (lastRoomCount > 1 && mapPictureBox.OpenedRoomCount == 1))
                        {
                            FloorStartTime = DateTimeOffset.Now.AddSeconds(-1);
                        }
                        if (mapPictureBox.OpenedRoomCount > 0)
                        {
                            // Found a floor size that aligns correctly with the rooms, this must be the right one.
                            lastRoomCount = mapPictureBox.OpenedRoomCount;
                            if (mapPictureBox.HomeLocation != MapUtils.NotFound)
                                lastHomeLocation = mapPictureBox.HomeLocation;
                            break;
                        }
                    }
                    else
                    {
                        // No map marker was even found, skip trying other floor sizes.
                        bmp.Dispose();
                        break;
                    }
                }
            }
        }

        private void UpdateDataLabel()
        {
            var minutes = GetElapsedTime().TotalMinutes;
            var roomsPerMinStr = ((mapPictureBox.OpenedRoomCount - 0.8) / minutes).ToString("0.0");
            var rrpmStr = ((mapPictureBox.OpenedRoomCount - 0.8 * (1 + mapPictureBox.LeafCount)) / minutes).ToString("0.0");
            dataLabel.Text = $"{mapPictureBox.OpenedRoomCount} rooms | {roomsPerMinStr} rpm | {mapPictureBox.LeafCount} dead ends | {rrpmStr} rrpm";
        }

        private TimeSpan GetElapsedTime()
        {
            return DateTimeOffset.Now - FloorStartTime;
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

            if (FloorStartTime != DateTimeOffset.MinValue)
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
            SaveMap();
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
            FloorStartTime = DateTimeOffset.Now;
        }

        private void plusOneOrTenButton_Click(object sender, EventArgs e)
        {
            if (FloorStartTime != DateTimeOffset.MinValue)
            {
                if (sender == plusOneButton)
                    FloorStartTime = FloorStartTime.AddSeconds(-1);
                else if (sender == plusTenButton)
                    FloorStartTime = FloorStartTime.AddSeconds(-10);
                else
                    FloorStartTime = FloorStartTime.AddSeconds(10);
            }
            UpdateTimerLabel();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WinterfaceButton_Click(object sender, EventArgs e)
        {
            winterfaceWindow.Show();
            winterfaceWindow.Focus();
        }
    }
}
