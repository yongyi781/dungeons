﻿using System;
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
        private bool isPaused = false;
        private DateTimeOffset timerCheckpoint = DateTimeOffset.Now;

        public Form1()
        {
            InitializeComponent();

            MapUtils.InitializeSignatures();
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Start on top right, lol
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width + 5, 0);
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
                if (HasMap(bmp))
                {
                    statusLabel.Text = $"Updated map from {mapLocation}.";
                    ResumeTimer();
                    mapPictureBox.Image = bmp;
                    saveMapButton.Enabled = true;
                    var prevHomeLocation = mapPictureBox.HomeLocation;
                    mapPictureBox.BuildMap();
                    UpdateDataLabel();

                    // Reset when home changes
                    if (mapPictureBox.HomeLocation != MapUtils.NotFound && mapPictureBox.HomeLocation != prevHomeLocation)
                    {
                        timerCheckpoint = DateTimeOffset.Now.AddSeconds(-3);
                    }
                }
                else
                {
                    statusLabel.Text = $"Waiting for map at {mapLocation}.";
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
            return DateTimeOffset.Now - timerCheckpoint;
        }

        private void findMapButton_Click(object sender, EventArgs e)
        {
            var mapLocation = FindMap();
            if (mapLocation != MapUtils.NotFound)
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

            timerLabel.Text = "Timer: " + (DateTimeOffset.Now - timerCheckpoint).ToString("m':'ss");
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

        private void distancesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mapPictureBox.DrawDistancesEnabled = distancesCheckBox.Checked;
            mapPictureBox.Invalidate();
        }

        private void resetTimerButton_Click(object sender, EventArgs e)
        {
            timerCheckpoint = DateTimeOffset.Now;
        }
    }
}
