using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeons
{
    public partial class MapForm : Form
    {
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        private Point lastHomeLocation = MapUtils.Invalid;
        private int lastRoomCount = 0;
        private Point mapLocation = MapUtils.Invalid;
        private readonly MainForm dataWindow;

        private static readonly Keys[] KeysToEat =
        {
            Keys.Enter,
            Keys.Space,
            Keys.Up,
            Keys.Left,
            Keys.Right,
            Keys.Down
        };

        private static readonly Dictionary<FloorSize, Size> rsMapSizes = new Dictionary<FloorSize, Size>
        {
            [FloorSize.Small] = new Size(140, 140),
            [FloorSize.Medium] = new Size(140, 280),
            [FloorSize.Large] = new Size(280, 280)
        };

        public MapForm(MainForm dataWindow)
        {
            InitializeComponent();

            FontType.InitializeFonts();
            mapPictureBox.FloorSize = FloorSize;
            this.dataWindow = dataWindow;
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
                Log("Imported settings from previous version");
            }
        }

        public DateTimeOffset FloorStartTime { get; private set; } = DateTimeOffset.MinValue;
        public int Roomcount => mapPictureBox.GameMap.OpenedRoomCount;
        public int LeafCount => mapPictureBox.GameMap.DeadEndCount;

        public ProcessWindow RSWindow => dataWindow.SelectedWindow;

        public FloorSize FloorSize
        {
            get => mapPictureBox.FloorSize;
            set => mapPictureBox.FloorSize = value;
        }

        public void SaveMap()
        {
            Directory.CreateDirectory(Properties.Settings.Default.MapSaveLocation);
            if (mapPictureBox.Image != null && Directory.Exists(Properties.Settings.Default.MapSaveLocation))
            {
                var fileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                mapPictureBox.Image.Save(Path.Combine(Properties.Settings.Default.MapSaveLocation, $"map_{fileName}.png"));
                Log("Map saved!");
            }
        }

        public async Task CalibrateAsync()
        {
            var (mapLocation, floorSize) = await FindMapAsync();
            if (mapLocation != MapUtils.Invalid)
            {
                Log($"Calibrated! Map location = {mapLocation}, Size = {floorSize}");
                this.mapLocation = mapLocation;
                UpdateMap();
            }
            else
            {
                Log($"Could not find map. Current map search location = {this.mapLocation}");
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
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }

        private async Task<(Point, FloorSize)> FindMapAsync()
        {
            var window = dataWindow.SelectedWindow;
            if (window != null && !window.HasExited)
            {
                using var bmp = window.Capture();
                // Search for map marker
                foreach (var floorSize in FloorSize.RSSizes)
                {
                    var match = await Task.Run(() => UnsafeBitmap.FindMapByCorners(bmp, rsMapSizes[floorSize]));
                    if (match != MapUtils.Invalid)
                    {
                        return (match, floorSize);
                    }
                }
            }

            return (MapUtils.Invalid, FloorSize.Small);
        }

        private void UpdateMap()
        {
            var window = dataWindow.SelectedWindow;
            if (window == null || window.HasExited)
                return;

            foreach (var floorSize in FloorSize.RSSizes)
            {
                if (mapLocation != MapUtils.Invalid)
                {
                    var mapSize = rsMapSizes[floorSize];
                    var bmp = window.Capture(new Rectangle(mapLocation, mapSize));
                    if (bmp == null)
                        return; // Break out of the loop, window capture won't work for the other cases either.

                    if (MapReader.IsValidInGameMap(bmp))
                    {
                        FloorSize = floorSize;
                        mapPictureBox.Size = mapSize;
                        timer.Start();
                        if (mapPictureBox.Image != null)
                            mapPictureBox.Image.Dispose();
                        mapPictureBox.Image = bmp;
                        mapPictureBox.ReadMap();
                        UpdateDataLabel();

                        // Reset when home changes or on first map load
                        if (FloorStartTime == DateTimeOffset.MinValue || (mapPictureBox.GameMap.Base != MapUtils.Invalid
                            && lastHomeLocation != MapUtils.Invalid
                            && mapPictureBox.GameMap.Base != lastHomeLocation)
                            || (lastRoomCount > 1 && mapPictureBox.GameMap.OpenedRoomCount == 1))
                        {
                            FloorStartTime = DateTimeOffset.Now.AddSeconds(-1);
                        }
                        // Found a floor size that aligns correctly with the rooms, this must be the right one.
                        lastRoomCount = mapPictureBox.GameMap.OpenedRoomCount;
                        if (mapPictureBox.GameMap.Base != MapUtils.Invalid)
                            lastHomeLocation = mapPictureBox.GameMap.Base;
                        break;
                    }
                    else
                    {
                        bmp.Dispose();
                    }
                }
            }
        }

        private void UpdateDataLabel()
        {
            var minutes = GetElapsedTime().TotalMinutes;
            var roomsPerMinStr = ((mapPictureBox.GameMap.OpenedRoomCount - 0.8) / minutes).ToString("0.0");
            var rrpmStr = ((mapPictureBox.GameMap.OpenedRoomCount - 0.8 * (1 + mapPictureBox.GameMap.DeadEndCount)) / minutes).ToString("0.0");
            dataLabel.Text = $"{mapPictureBox.GameMap.OpenedRoomCount} rooms | {roomsPerMinStr} rpm | {mapPictureBox.GameMap.DeadEndCount} dead ends | {rrpmStr} rrpm";
        }

        private TimeSpan GetElapsedTime()
        {
            return DateTimeOffset.Now - FloorStartTime;
        }

        private void Log(string text)
        {
            dataWindow.Log(text);
        }

        private async void CalibrateButton_Click(object sender, EventArgs e)
        {
            await CalibrateAsync();
        }

        private void Timer_Tick(object sender, EventArgs e)
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

        private void SaveMapButton_Click(object sender, EventArgs e)
        {
            SaveMap();
        }

        private void MapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateDataLabel();
        }

        private void ClearAnnotationsButton_Click(object sender, EventArgs e)
        {
            mapPictureBox.ClearAnnotations();
        }

        private void TopMostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = topMostCheckBox.Checked;
        }

        private void ResetTimerButton_Click(object sender, EventArgs e)
        {
            FloorStartTime = DateTimeOffset.Now;
        }

        private void PlusOneOrTenButton_Click(object sender, EventArgs e)
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

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
