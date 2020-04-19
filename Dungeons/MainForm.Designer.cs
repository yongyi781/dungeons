namespace Dungeons
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Partners = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Floor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FloorSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FloorXP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrestigeXP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BaseXP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SizeMod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BonusMod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelMod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DifficultyMod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalMod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinalXP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Boss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Roomcount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeadEnds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveMapButton = new System.Windows.Forms.Button();
            this.windowComboBox = new System.Windows.Forms.ComboBox();
            this.browseWinterfaceSaveLocationButton = new System.Windows.Forms.Button();
            this.winterfaceSaveLocationTextBox = new System.Windows.Forms.TextBox();
            this.winterfaceSaveLocationLabel = new System.Windows.Forms.Label();
            this.browseMapSaveLocationButton = new System.Windows.Forms.Button();
            this.mapSaveLocationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveImagesCheckBox = new System.Windows.Forms.CheckBox();
            this.captureButton = new System.Windows.Forms.Button();
            this.multiplayerColumnsRadioButton = new System.Windows.Forms.RadioButton();
            this.soloColumnsRadioButton = new System.Windows.Forms.RadioButton();
            this.mapFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.winterfaceFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.logTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Timestamp,
            this.Partners,
            this.Floor,
            this.FloorSize,
            this.Time,
            this.FloorXP,
            this.PrestigeXP,
            this.BaseXP,
            this.SizeMod,
            this.BonusMod,
            this.LevelMod,
            this.DifficultyMod,
            this.TotalMod,
            this.FinalXP,
            this.Boss,
            this.Roomcount,
            this.DeadEnds});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 93);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(765, 198);
            this.dataGridView1.TabIndex = 1;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Width = 5;
            // 
            // Timestamp
            // 
            this.Timestamp.HeaderText = "Timestamp";
            this.Timestamp.Name = "Timestamp";
            this.Timestamp.Width = 5;
            // 
            // Partners
            // 
            this.Partners.HeaderText = "Partners";
            this.Partners.Name = "Partners";
            this.Partners.Width = 5;
            // 
            // Floor
            // 
            this.Floor.HeaderText = "Floor";
            this.Floor.Name = "Floor";
            this.Floor.Width = 5;
            // 
            // FloorSize
            // 
            this.FloorSize.HeaderText = "Size";
            this.FloorSize.Name = "FloorSize";
            this.FloorSize.Width = 5;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.Width = 5;
            // 
            // FloorXP
            // 
            this.FloorXP.HeaderText = "Floor XP";
            this.FloorXP.Name = "FloorXP";
            this.FloorXP.Width = 5;
            // 
            // PrestigeXP
            // 
            this.PrestigeXP.HeaderText = "Prestige XP";
            this.PrestigeXP.Name = "PrestigeXP";
            this.PrestigeXP.Width = 5;
            // 
            // BaseXP
            // 
            this.BaseXP.HeaderText = "Base XP";
            this.BaseXP.Name = "BaseXP";
            this.BaseXP.Width = 5;
            // 
            // SizeMod
            // 
            this.SizeMod.HeaderText = "Size +%";
            this.SizeMod.Name = "SizeMod";
            this.SizeMod.Width = 5;
            // 
            // BonusMod
            // 
            this.BonusMod.HeaderText = "Bonus +%";
            this.BonusMod.Name = "BonusMod";
            this.BonusMod.Width = 5;
            // 
            // LevelMod
            // 
            this.LevelMod.HeaderText = "Level mod %";
            this.LevelMod.Name = "LevelMod";
            this.LevelMod.Width = 5;
            // 
            // DifficultyMod
            // 
            this.DifficultyMod.HeaderText = "Difficulty %";
            this.DifficultyMod.Name = "DifficultyMod";
            this.DifficultyMod.Width = 5;
            // 
            // TotalMod
            // 
            this.TotalMod.HeaderText = "Total Mod";
            this.TotalMod.Name = "TotalMod";
            this.TotalMod.Width = 5;
            // 
            // FinalXP
            // 
            this.FinalXP.HeaderText = "FinalXP";
            this.FinalXP.Name = "FinalXP";
            this.FinalXP.Width = 5;
            // 
            // Boss
            // 
            this.Boss.HeaderText = "Boss";
            this.Boss.Name = "Boss";
            this.Boss.Width = 5;
            // 
            // Roomcount
            // 
            this.Roomcount.HeaderText = "Roomcount";
            this.Roomcount.Name = "Roomcount";
            this.Roomcount.Width = 5;
            // 
            // DeadEnds
            // 
            this.DeadEnds.HeaderText = "Dead Ends";
            this.DeadEnds.Name = "DeadEnds";
            this.DeadEnds.Width = 5;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.saveMapButton);
            this.panel1.Controls.Add(this.windowComboBox);
            this.panel1.Controls.Add(this.browseWinterfaceSaveLocationButton);
            this.panel1.Controls.Add(this.winterfaceSaveLocationTextBox);
            this.panel1.Controls.Add(this.winterfaceSaveLocationLabel);
            this.panel1.Controls.Add(this.browseMapSaveLocationButton);
            this.panel1.Controls.Add(this.mapSaveLocationTextBox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.saveImagesCheckBox);
            this.panel1.Controls.Add(this.captureButton);
            this.panel1.Controls.Add(this.multiplayerColumnsRadioButton);
            this.panel1.Controls.Add(this.soloColumnsRadioButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(765, 93);
            this.panel1.TabIndex = 0;
            // 
            // saveMapButton
            // 
            this.saveMapButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.saveMapButton.Location = new System.Drawing.Point(335, 10);
            this.saveMapButton.Name = "saveMapButton";
            this.saveMapButton.Size = new System.Drawing.Size(75, 23);
            this.saveMapButton.TabIndex = 2;
            this.saveMapButton.Text = "Save Map";
            this.saveMapButton.UseVisualStyleBackColor = true;
            this.saveMapButton.Click += new System.EventHandler(this.SaveMapButton_Click);
            // 
            // windowComboBox
            // 
            this.windowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.windowComboBox.FormattingEnabled = true;
            this.windowComboBox.Location = new System.Drawing.Point(11, 11);
            this.windowComboBox.Name = "windowComboBox";
            this.windowComboBox.Size = new System.Drawing.Size(162, 23);
            this.windowComboBox.TabIndex = 0;
            this.windowComboBox.DropDown += new System.EventHandler(this.ComboBox1_DropDown);
            this.windowComboBox.SelectionChangeCommitted += new System.EventHandler(this.ComboBox1_SelectionChangeCommitted);
            // 
            // browseWinterfaceSaveLocationButton
            // 
            this.browseWinterfaceSaveLocationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.browseWinterfaceSaveLocationButton.Location = new System.Drawing.Point(572, 40);
            this.browseWinterfaceSaveLocationButton.Name = "browseWinterfaceSaveLocationButton";
            this.browseWinterfaceSaveLocationButton.Size = new System.Drawing.Size(24, 23);
            this.browseWinterfaceSaveLocationButton.TabIndex = 10;
            this.browseWinterfaceSaveLocationButton.Text = "...";
            this.browseWinterfaceSaveLocationButton.UseVisualStyleBackColor = true;
            this.browseWinterfaceSaveLocationButton.Click += new System.EventHandler(this.browseWinterfaceSaveLocationButton_Click);
            // 
            // winterfaceSaveLocationTextBox
            // 
            this.winterfaceSaveLocationTextBox.Location = new System.Drawing.Point(435, 41);
            this.winterfaceSaveLocationTextBox.Name = "winterfaceSaveLocationTextBox";
            this.winterfaceSaveLocationTextBox.ReadOnly = true;
            this.winterfaceSaveLocationTextBox.Size = new System.Drawing.Size(131, 23);
            this.winterfaceSaveLocationTextBox.TabIndex = 9;
            // 
            // winterfaceSaveLocationLabel
            // 
            this.winterfaceSaveLocationLabel.AutoSize = true;
            this.winterfaceSaveLocationLabel.Location = new System.Drawing.Point(290, 44);
            this.winterfaceSaveLocationLabel.Name = "winterfaceSaveLocationLabel";
            this.winterfaceSaveLocationLabel.Size = new System.Drawing.Size(139, 15);
            this.winterfaceSaveLocationLabel.TabIndex = 8;
            this.winterfaceSaveLocationLabel.Text = "Winterface save location:";
            // 
            // browseMapSaveLocationButton
            // 
            this.browseMapSaveLocationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.browseMapSaveLocationButton.Location = new System.Drawing.Point(260, 40);
            this.browseMapSaveLocationButton.Name = "browseMapSaveLocationButton";
            this.browseMapSaveLocationButton.Size = new System.Drawing.Size(24, 23);
            this.browseMapSaveLocationButton.TabIndex = 7;
            this.browseMapSaveLocationButton.Text = "...";
            this.browseMapSaveLocationButton.UseVisualStyleBackColor = true;
            this.browseMapSaveLocationButton.Click += new System.EventHandler(this.browseMapSaveLocationButton_Click);
            // 
            // mapSaveLocationTextBox
            // 
            this.mapSaveLocationTextBox.Location = new System.Drawing.Point(123, 41);
            this.mapSaveLocationTextBox.Name = "mapSaveLocationTextBox";
            this.mapSaveLocationTextBox.ReadOnly = true;
            this.mapSaveLocationTextBox.Size = new System.Drawing.Size(131, 23);
            this.mapSaveLocationTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Map save location:";
            // 
            // saveImagesCheckBox
            // 
            this.saveImagesCheckBox.AutoSize = true;
            this.saveImagesCheckBox.Checked = true;
            this.saveImagesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveImagesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.saveImagesCheckBox.Location = new System.Drawing.Point(416, 12);
            this.saveImagesCheckBox.Name = "saveImagesCheckBox";
            this.saveImagesCheckBox.Size = new System.Drawing.Size(148, 20);
            this.saveImagesCheckBox.TabIndex = 4;
            this.saveImagesCheckBox.Text = "Save maps on capture";
            this.saveImagesCheckBox.UseVisualStyleBackColor = true;
            // 
            // captureButton
            // 
            this.captureButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.captureButton.Location = new System.Drawing.Point(179, 10);
            this.captureButton.Name = "captureButton";
            this.captureButton.Size = new System.Drawing.Size(150, 23);
            this.captureButton.TabIndex = 3;
            this.captureButton.Text = "Capture/Calibrate (F11)";
            this.captureButton.UseVisualStyleBackColor = true;
            this.captureButton.Click += new System.EventHandler(this.CaptureButton_Click);
            // 
            // multiplayerColumnsRadioButton
            // 
            this.multiplayerColumnsRadioButton.AutoSize = true;
            this.multiplayerColumnsRadioButton.Checked = true;
            this.multiplayerColumnsRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.multiplayerColumnsRadioButton.Location = new System.Drawing.Point(115, 70);
            this.multiplayerColumnsRadioButton.Name = "multiplayerColumnsRadioButton";
            this.multiplayerColumnsRadioButton.Size = new System.Drawing.Size(140, 20);
            this.multiplayerColumnsRadioButton.TabIndex = 12;
            this.multiplayerColumnsRadioButton.TabStop = true;
            this.multiplayerColumnsRadioButton.Text = "&Multiplayer columns";
            this.multiplayerColumnsRadioButton.UseVisualStyleBackColor = true;
            this.multiplayerColumnsRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckedChanged);
            // 
            // soloColumnsRadioButton
            // 
            this.soloColumnsRadioButton.AutoSize = true;
            this.soloColumnsRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.soloColumnsRadioButton.Location = new System.Drawing.Point(11, 70);
            this.soloColumnsRadioButton.Name = "soloColumnsRadioButton";
            this.soloColumnsRadioButton.Size = new System.Drawing.Size(103, 20);
            this.soloColumnsRadioButton.TabIndex = 11;
            this.soloColumnsRadioButton.Text = "&Solo columns";
            this.soloColumnsRadioButton.UseVisualStyleBackColor = true;
            this.soloColumnsRadioButton.CheckedChanged += new System.EventHandler(this.modeRadioButton_CheckedChanged);
            // 
            // mapFolderBrowserDialog
            // 
            this.mapFolderBrowserDialog.Description = "Select map save location.";
            // 
            // winterfaceFolderBrowserDialog
            // 
            this.winterfaceFolderBrowserDialog.Description = "Select winterface save location.";
            // 
            // logTextBox
            // 
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 10F);
            this.logTextBox.Location = new System.Drawing.Point(0, 291);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logTextBox.Size = new System.Drawing.Size(765, 129);
            this.logTextBox.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(765, 420);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.logTextBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Dungeons";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton multiplayerColumnsRadioButton;
        private System.Windows.Forms.RadioButton soloColumnsRadioButton;
        private System.Windows.Forms.Button captureButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Partners;
        private System.Windows.Forms.DataGridViewTextBoxColumn Floor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FloorSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn FloorXP;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrestigeXP;
        private System.Windows.Forms.DataGridViewTextBoxColumn BaseXP;
        private System.Windows.Forms.DataGridViewTextBoxColumn SizeMod;
        private System.Windows.Forms.DataGridViewTextBoxColumn BonusMod;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelMod;
        private System.Windows.Forms.DataGridViewTextBoxColumn DifficultyMod;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalMod;
        private System.Windows.Forms.DataGridViewTextBoxColumn FinalXP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Boss;
        private System.Windows.Forms.DataGridViewTextBoxColumn Roomcount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeadEnds;
        private System.Windows.Forms.CheckBox saveImagesCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mapSaveLocationTextBox;
        private System.Windows.Forms.Button browseMapSaveLocationButton;
        private System.Windows.Forms.Button browseWinterfaceSaveLocationButton;
        private System.Windows.Forms.TextBox winterfaceSaveLocationTextBox;
        private System.Windows.Forms.Label winterfaceSaveLocationLabel;
        private System.Windows.Forms.FolderBrowserDialog mapFolderBrowserDialog;
        private System.Windows.Forms.FolderBrowserDialog winterfaceFolderBrowserDialog;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.ComboBox windowComboBox;
        private System.Windows.Forms.Button saveMapButton;
    }
}