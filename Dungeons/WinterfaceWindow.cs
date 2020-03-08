using Dungeons.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Dungeons
{
    public partial class WinterfaceWindow : Form
    {
        static readonly Point NotFound = new Point(-1, -1);
        static readonly Point DefaultOffset = new Point(710, 330);

        private readonly Form1 parent;

        public WinterfaceWindow(Form1 parent)
        {
            InitializeComponent();

            this.parent = parent;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            UpdateSaveLocationTextBoxes();
        }

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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            NativeMethods.RegisterHotKey(Handle, 0, 0, NativeMethods.VK_F11);
            dataGridView1.Font = new Font("Calibri", 11);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_HOTKEY:
                    var row = GrabFromScreen();
                    if (row != null)
                    {
                        var visibleCellValues = from DataGridViewCell cell in row.Cells
                                                where cell.Visible
                                                select cell.Value;
                        Clipboard.SetText(string.Join("\t", visibleCellValues));
                        if (saveImagesCheckBox.Checked)
                            parent.SaveMap();
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private DataGridViewRow GrabFromScreen()
        {
            var size = SystemInformation.VirtualScreen.Size;
            Dictionary<string, string> dict;
            using (var bmp = new Bitmap(size.Width, size.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, 0, 0, size);
                }

                dict = ParseBitmap(bmp, saveImagesCheckBox.Checked);
            }
            if (dict == null)
                return null;

            if (dict["DifficultyMod"] == "+0")
            {
                // Change to solo if it's a 1:1
                soloColumnsRadioButton.Checked = true;
                if (dict["FloorSize"] == "Large")
                    dict["FloorSize"] = "Large2";
            }
            return AddRow(dict);
        }

        private Dictionary<string, string> ParseBitmap(Bitmap bmp, bool saveToFile = false)
        {
            using (var b = new UnsafeBitmap(bmp, ImageLockMode.ReadOnly))
            {
                Point p;
                var firstPointToTry = new Point(710, 330);
                if (b.IsMatch(Properties.Resources.WinterfaceMarker, DefaultOffset.X, DefaultOffset.Y, 0))
                    p = DefaultOffset;
                else
                    p = b.FindMatch(Properties.Resources.WinterfaceMarker, 0);

                if (p == NotFound)
                {
                    return null;
                }
                var w = new Winterface(b, p.X, p.Y);

                var fields = new List<Field> { Field.Time, Field.FloorXP, Field.PrestigeXP, Field.AverageBaseXP, Field.SizeMod, Field.BonusMod, Field.LevelMod };
                fields.Add(Field.DifficultyMod);
                fields.Add(Field.TotalMod);
                fields.Add(Field.FinalXP);

                var row = (from f in fields
                           select new { Key = f.Name, Value = w.ReadField(f) }).ToDictionary(a => a.Key, a => a.Value);

                var floor = w.ReadField(Field.Floor);
                var redFloor = w.ReadField(Field.RedFloor);
                var floorSizeMod = row["SizeMod"];
                var sizeText = floorSizeMod == "+15" ? "Large" : floorSizeMod == "+7" ? "Medium" : "Small";
                row["Floor"] = floor + redFloor;
                row["FloorSize"] = sizeText;
                if (parent != null)
                {
                    row["Roomcount"] = parent.Roomcount.ToString();
                    row["DeadEnds"] = parent.LeafCount.ToString();
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

        private void CaptureButton_Click(object sender, EventArgs e)
        {
            GrabFromScreen();
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
    }
}
