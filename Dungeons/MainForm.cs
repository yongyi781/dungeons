using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeons
{
    public partial class MainForm : Form
    {
        static readonly Point NotFound = new(-1, -1);
        static readonly Point DefaultOffset = new(710, 330);

        private readonly MapForm mapForm;

        public MainForm()
        {
            InitializeComponent();

            mapForm = new MapForm(this);
            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            UpdateSaveLocationTextBoxes();
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
                Log("Imported settings from previous version");
            }
        }

        public ProcessWindow SelectedWindow => windowComboBox.SelectedItem as ProcessWindow;

        public DataGridViewRow AddRow(Dictionary<string, string> row)
        {
            if (row == null)
                return null;

            var gridRow = dataGridView1.Rows[dataGridView1.Rows.Add()];
            foreach (var pair in row)
            {
                gridRow.Cells[pair.Key] = new DataGridViewTextBoxCell { Value = pair.Value };
            }
            return gridRow;
        }

        public void Log(string text)
        {
            logTextBox.AppendText(text + Environment.NewLine);
        }

        public void RefreshProcessesList()
        {
            var selectedItem = windowComboBox.SelectedItem;
            var list = (from x in Process.GetProcessesByName("rs2client") select new ProcessWindow(x)).ToList();
#if DEBUG
            list.Add(new ProcessWindow(null));
#endif
            windowComboBox.DataSource = list;
            windowComboBox.SelectedItem = selectedItem;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            NativeMethods.RegisterHotKey(Handle, 0, 0, NativeMethods.VK_F11);
            dataGridView1.Font = new Font("Calibri", 11);

            windowComboBox.DataSource = (from x in Process.GetProcessesByName("rs2client") select new ProcessWindow(x)).ToList();
            Log("Started up, calibrating");
            _ = mapForm.CalibrateAsync();
            if (Screen.FromPoint(Properties.Settings.Default.MainFormLocation) != null)
            {
                Location = Properties.Settings.Default.MainFormLocation;
                if (!this.IsOnScreen())
                {
                    Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, 0);
                }
            }
            mapForm.Show(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Properties.Settings.Default.MainFormLocation = Location;
            Properties.Settings.Default.MapFormLocation = mapForm.Location;
            Properties.Settings.Default.Save();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_HOTKEY:
                    captureButton.PerformClick();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// Captures the winterface.
        /// </summary>
        /// <returns>true if winterface was found; otherwise, false.</returns>
        private async Task<bool> CaptureWinterfaceAsync()
        {

            using (var bmp = mapForm.RSWindow?.Capture())
            {
                if (bmp == null)
                    return false;
                var dict = await Task.Run(() => ParseBitmap(bmp, saveImagesCheckBox.Checked));
                if (dict == null)
                    return false;

                if (dict["DifficultyMod"] == "+0")
                {
                    // Change to solo if it's a 1:1
                    soloColumnsRadioButton.Checked = true;
                    if (dict["FloorSize"] == "Large")
                        dict["FloorSize"] = "Large2";
                }
                if (saveImagesCheckBox.Checked)
                    mapForm.SaveMap();
                var row = AddRow(dict);
                if (row != null)
                {
                    var visibleCellValues = from DataGridViewCell cell in row.Cells
                                            where cell.Visible
                                            select cell.Value;
                    Clipboard.SetText(string.Join("\t", visibleCellValues));
                }
            }
            return true;
        }

        private Dictionary<string, string> ParseBitmap(Bitmap bmp, bool saveToFile = false)
        {
            using var b = new UnsafeBitmap(bmp, ImageLockMode.ReadOnly);
            Point p;
            if (b.IsMatch(Properties.Resources.WinterfaceMarker, DefaultOffset.X, DefaultOffset.Y, 0))
                p = DefaultOffset;
            else
                p = b.FindMatch(Properties.Resources.WinterfaceMarker, 0);

            if (p == NotFound)
            {
                return null;
            }
            var w = new Winterface(b, p.X, p.Y);

            var fields = new List<Field>
            {
                Field.Time,
                Field.FloorXP,
                Field.PrestigeXP,
                Field.AverageBaseXP,
                Field.SizeMod,
                Field.BonusMod,
                Field.LevelMod,
                Field.DifficultyMod,
                Field.TotalMod,
                Field.FinalXP
            };

            var row = (from f in fields
                       select new { Key = f.Name, Value = w.ReadField(f) }).ToDictionary(a => a.Key, a => a.Value);

            var floor = w.ReadField(Field.Floor);
            var redFloor = w.ReadField(Field.RedFloor);
            var floorSizeMod = row["SizeMod"];
            var sizeText = floorSizeMod == "+15" ? "Large" : floorSizeMod == "+7" ? "Medium" : "Small";
            row["Floor"] = floor + redFloor;
            row["FloorSize"] = sizeText;
            if (mapForm != null)
            {
                row["Roomcount"] = mapForm.Roomcount.ToString();
                row["DeadEnds"] = mapForm.LeafCount.ToString();
            }
            var now = DateTime.Now;
            row["Timestamp"] = now.ToString();

            Directory.CreateDirectory(Properties.Settings.Default.WinterfaceSaveLocation);
            if (saveToFile && Directory.Exists(Properties.Settings.Default.WinterfaceSaveLocation))
            {
                // The \\g is because g is a date format character
                w.Save(Path.Combine(Properties.Settings.Default.WinterfaceSaveLocation, now.ToString("yyyy-MM-dd HH-mm-ss.pn\\g")));
            }

            return row;
        }

        private void UpdateSaveLocationTextBoxes()
        {
            mapSaveLocationTextBox.Text = Properties.Settings.Default.MapSaveLocation;
            winterfaceSaveLocationTextBox.Text = Properties.Settings.Default.WinterfaceSaveLocation;
        }

        private void modeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns["DifficultyMod"].Visible = multiplayerColumnsRadioButton.Checked;
            dataGridView1.Columns["Partners"].Visible = multiplayerColumnsRadioButton.Checked;
        }

        private void browseMapSaveLocationButton_Click(object sender, EventArgs e)
        {
            if (mapFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.MapSaveLocation = mapFolderBrowserDialog.SelectedPath;
                Properties.Settings.Default.Save();
                UpdateSaveLocationTextBoxes();
            }
        }

        private void browseWinterfaceSaveLocationButton_Click(object sender, EventArgs e)
        {
            if (winterfaceFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.WinterfaceSaveLocation = winterfaceFolderBrowserDialog.SelectedPath;
                Properties.Settings.Default.Save();
                UpdateSaveLocationTextBoxes();
            }
        }

        private void ComboBox1_DropDown(object sender, EventArgs e)
        {
            RefreshProcessesList();
        }

        private async void ComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Log("Selected index changed, calibrating");
            await mapForm.CalibrateAsync();
        }

        private void SaveMapButton_Click(object sender, EventArgs e)
        {
            mapForm.SaveMap();
        }

        private async void CaptureButton_Click(object sender, EventArgs e)
        {
            if (!await CaptureWinterfaceAsync())
                await mapForm.CalibrateAsync();
        }

        private void hideMapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            mapForm.SetShowMapStatsOnly(hideMapCheckBox.Checked);
        }
    }
}
