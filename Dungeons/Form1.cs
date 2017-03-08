using System;
using System.ComponentModel;
using System.Diagnostics;
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

        private static readonly Keys[] KeysToEat =
        {
            Keys.Enter,
            Keys.Space,
            Keys.Up,
            Keys.Left,
            Keys.Right,
            Keys.Down
        };

        private static readonly Point NotFound = new Point(-1, -1);

        private Bitmap mapMarker = Properties.Resources.MapMarker;
        private Point mapLocation = new Point(2703, 370);
        private bool isPaused = false;

        public Form1()
        {
            InitializeComponent();
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

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            mapPictureBox.ProcessKeyPress(e);
            base.OnKeyPress(e);
        }

        private unsafe bool IsMatch(BitmapData bmpData, BitmapData templateData, int offX, int offY)
        {
            var bmpScan0 = (byte*)bmpData.Scan0.ToPointer();
            var templateScan0 = (byte*)templateData.Scan0.ToPointer();

            int index = offY * bmpData.Stride + offX * 3;
            for (int i = 0; i < templateData.Width * 3; i++)
            {
                if (bmpScan0[index + i] != templateScan0[i])
                    return false;
            }
            return true;
        }

        private unsafe bool HasMap(Bitmap bmp, int mapOffsetX = MapOffsetX, int mapOffsetY = MapOffsetY)
        {
            using (var unsafeBmp = new UnsafeBitmap(bmp, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb))
            {
                using (var unsafeMarker = new UnsafeBitmap(mapMarker, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb))
                {
                    return IsMatch(unsafeBmp.BitmapData, unsafeMarker.BitmapData, mapOffsetX, mapOffsetY);
                }
            }
        }

        // Assumes map marker has height of 1
        private unsafe Point FindMapMarker(Bitmap bmp)
        {
            using (var unsafeBmp = new UnsafeBitmap(bmp, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb))
            {
                using (var unsafeMarker = new UnsafeBitmap(mapMarker, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb))
                {
                    var bmpScan0 = (byte*)unsafeBmp.BitmapData.Scan0.ToPointer();
                    var mapMarkerScan0 = (byte*)unsafeMarker.BitmapData.Scan0.ToPointer();

                    for (int offY = 0; offY < unsafeBmp.BitmapData.Height; offY++)
                    {
                        for (int offX = 0; offX < unsafeBmp.BitmapData.Width - unsafeMarker.BitmapData.Width + 1; offX++)
                        {
                            if (IsMatch(unsafeBmp.BitmapData, unsafeMarker.BitmapData, offX, offY) && !DesktopBounds.Contains(offX, offY))
                                return new Point(offX, offY);
                        }
                    }
                }
            }

            return new Point(-1, -1);
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
            return FindMapMarker(bmp);
        }

        private void ResumeTimer()
        {
            pauseButton.Enabled = true;
            pauseButton.Text = "Pause";
            pauseButton.ForeColor = Color.Maroon;
            isPaused = false;
            timer.Start();
        }

        private void PauseTimer()
        {
            pauseButton.Text = "Resume";
            pauseButton.ForeColor = Color.Green;
            statusLabel.Text = "Paused.";
            isPaused = true;
            timer.Stop();
        }

        private void UpdateMap()
        {
            var bmp = new Bitmap(MapWidth, MapHeight);

            if (mapLocation != NotFound)
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
                if (HasMap(bmp))
                {
                    statusLabel.Text = $"Updated map from {mapLocation}.";
                    ResumeTimer();
                    mapPictureBox.Image = bmp;
                    saveMapButton.Enabled = true;
                    UpdateDataLabel();
                }
                else
                {
                    statusLabel.Text = $"Waiting for map at {mapLocation}.";
                }
            }
        }

        //private int CountRooms()
        //{
        //    var bitmap = mapPictureBox.Image as Bitmap;
        //    if (bitmap == null)
        //        return 0;

        //    int count = 0;
        //    for (int y = 1; y <= 8; y++)
        //    {
        //        for (int x = 1; x <= 8; x++)
        //        {
        //            var p = MapToClientCoords(new Point(x, y));
        //            // Take average of 4 colors
        //            Color[] colors = {
        //                bitmap.GetPixel(p.X + 8, p.Y + 7),
        //                bitmap.GetPixel(p.X + 8, p.Y + 25),
        //                bitmap.GetPixel(p.X + 24, p.Y + 7),
        //                bitmap.GetPixel(p.X + 24, p.Y + 25)
        //            };

        //            var averageColor = Color.FromArgb(
        //                (int)colors.Average(c => c.R),
        //                (int)colors.Average(c => c.G),
        //                (int)colors.Average(c => c.B));

        //            if (IsOpenedRoomColor(averageColor))
        //                ++count;
        //        }
        //    }

        //    return count;
        //}

        //private bool IsOpenedRoomColor(Color c)
        //{
        //    return c.R > 100 & c.R < 150 && c.G > 50 && c.G < 120 && c.B < 65;
        //}

        private void UpdateDataLabel()
        {
            dataLabel.Text = $"Selected: ({mapPictureBox.SelectedLocation.X}, {mapPictureBox.SelectedLocation.Y}) | Computed room type: {GetRoomType(mapPictureBox.SelectedLocation)}";
        }

        private string GetRoomType(Point p)
        {
            var bmp = mapPictureBox.Image as Bitmap;
            if (bmp == null)
                return string.Empty;
            var pc = MapUtils.MapToClientCoords(p);
            var c = bmp.GetPixel(pc.X + 16, pc.Y + 16);
            return c.R > 100 & c.R < 150 && c.G > 50 && c.G < 120 && c.B < 65 ? "Opened" : "Not opened";
        }

        private void findMapButton_Click(object sender, EventArgs e)
        {
            var mapLocation = FindMap();
            if (mapLocation != NotFound)
            {
                this.mapLocation = mapLocation;
                UpdateMap();
            }
            else
            {
                statusLabel.Text = "No map found.";
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateMap();
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

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (isPaused)
                ResumeTimer();
            else
                PauseTimer();
        }

        private void mapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateDataLabel();
        }

        private void clearAnnotationsButton_Click(object sender, EventArgs e)
        {
            mapPictureBox.ClearAnnotations();
        }
    }
}
