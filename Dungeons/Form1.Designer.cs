namespace Dungeons
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.calibrateButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dataLabel = new System.Windows.Forms.Label();
            this.saveMapButton = new System.Windows.Forms.Button();
            this.savedLabel = new System.Windows.Forms.Label();
            this.saveLabelHideTimer = new System.Windows.Forms.Timer(this.components);
            this.clearAnnotationsButton = new System.Windows.Forms.Button();
            this.distancesCheckBox = new System.Windows.Forms.CheckBox();
            this.timerLabel = new System.Windows.Forms.Label();
            this.resetTimerButton = new System.Windows.Forms.Button();
            this.plusTenButton = new System.Windows.Forms.Button();
            this.plusOneButton = new System.Windows.Forms.Button();
            this.mapPictureBox = new Dungeons.MapPictureBox();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // calibrateButton
            // 
            this.calibrateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.calibrateButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.calibrateButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.calibrateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.calibrateButton.Location = new System.Drawing.Point(12, 384);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 25);
            this.calibrateButton.TabIndex = 6;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = false;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // dataLabel
            // 
            this.dataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataLabel.AutoSize = true;
            this.dataLabel.Location = new System.Drawing.Point(9, 342);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(0, 15);
            this.dataLabel.TabIndex = 5;
            // 
            // saveMapButton
            // 
            this.saveMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.saveMapButton.Enabled = false;
            this.saveMapButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.saveMapButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.saveMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveMapButton.Location = new System.Drawing.Point(93, 384);
            this.saveMapButton.Name = "saveMapButton";
            this.saveMapButton.Size = new System.Drawing.Size(75, 25);
            this.saveMapButton.TabIndex = 7;
            this.saveMapButton.Text = "Save Map";
            this.saveMapButton.UseVisualStyleBackColor = false;
            this.saveMapButton.Click += new System.EventHandler(this.saveMapButton_Click);
            // 
            // savedLabel
            // 
            this.savedLabel.AutoSize = true;
            this.savedLabel.ForeColor = System.Drawing.Color.Lime;
            this.savedLabel.Location = new System.Drawing.Point(265, 420);
            this.savedLabel.Name = "savedLabel";
            this.savedLabel.Size = new System.Drawing.Size(41, 15);
            this.savedLabel.TabIndex = 10;
            this.savedLabel.Text = "Saved!";
            this.savedLabel.Visible = false;
            // 
            // saveLabelHideTimer
            // 
            this.saveLabelHideTimer.Interval = 1500;
            this.saveLabelHideTimer.Tick += new System.EventHandler(this.saveLabelHideTimer_Tick);
            // 
            // clearAnnotationsButton
            // 
            this.clearAnnotationsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.clearAnnotationsButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.clearAnnotationsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.clearAnnotationsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearAnnotationsButton.Location = new System.Drawing.Point(12, 415);
            this.clearAnnotationsButton.Name = "clearAnnotationsButton";
            this.clearAnnotationsButton.Size = new System.Drawing.Size(122, 25);
            this.clearAnnotationsButton.TabIndex = 8;
            this.clearAnnotationsButton.Text = "Clear Annotations";
            this.clearAnnotationsButton.UseVisualStyleBackColor = false;
            this.clearAnnotationsButton.Click += new System.EventHandler(this.clearAnnotationsButton_Click);
            // 
            // distancesCheckBox
            // 
            this.distancesCheckBox.AutoSize = true;
            this.distancesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.distancesCheckBox.Location = new System.Drawing.Point(140, 418);
            this.distancesCheckBox.Name = "distancesCheckBox";
            this.distancesCheckBox.Size = new System.Drawing.Size(82, 20);
            this.distancesCheckBox.TabIndex = 9;
            this.distancesCheckBox.Text = "Distances";
            this.distancesCheckBox.UseVisualStyleBackColor = true;
            this.distancesCheckBox.CheckedChanged += new System.EventHandler(this.distancesCheckBox_CheckedChanged);
            // 
            // timerLabel
            // 
            this.timerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timerLabel.Font = new System.Drawing.Font("Georgia", 12F);
            this.timerLabel.Location = new System.Drawing.Point(12, 313);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(160, 18);
            this.timerLabel.TabIndex = 1;
            this.timerLabel.Text = "0:00";
            this.timerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // resetTimerButton
            // 
            this.resetTimerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resetTimerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.resetTimerButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.resetTimerButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.resetTimerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetTimerButton.Location = new System.Drawing.Point(258, 311);
            this.resetTimerButton.Name = "resetTimerButton";
            this.resetTimerButton.Size = new System.Drawing.Size(48, 25);
            this.resetTimerButton.TabIndex = 4;
            this.resetTimerButton.Text = "Reset";
            this.resetTimerButton.UseVisualStyleBackColor = false;
            this.resetTimerButton.Click += new System.EventHandler(this.resetTimerButton_Click);
            // 
            // plusTenButton
            // 
            this.plusTenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.plusTenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.plusTenButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.plusTenButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.plusTenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusTenButton.Location = new System.Drawing.Point(218, 311);
            this.plusTenButton.Name = "plusTenButton";
            this.plusTenButton.Size = new System.Drawing.Size(37, 25);
            this.plusTenButton.TabIndex = 3;
            this.plusTenButton.Text = "+10";
            this.plusTenButton.UseVisualStyleBackColor = false;
            this.plusTenButton.Click += new System.EventHandler(this.plusOneOrTenButton_Click);
            // 
            // plusOneButton
            // 
            this.plusOneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.plusOneButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.plusOneButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.plusOneButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.plusOneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusOneButton.Location = new System.Drawing.Point(178, 311);
            this.plusOneButton.Name = "plusOneButton";
            this.plusOneButton.Size = new System.Drawing.Size(37, 25);
            this.plusOneButton.TabIndex = 2;
            this.plusOneButton.Text = "+1";
            this.plusOneButton.UseVisualStyleBackColor = false;
            this.plusOneButton.Click += new System.EventHandler(this.plusOneOrTenButton_Click);
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mapPictureBox.DrawDistancesEnabled = false;
            this.mapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.SelectedLocation = new System.Drawing.Point(0, 0);
            this.mapPictureBox.Size = new System.Drawing.Size(318, 310);
            this.mapPictureBox.TabIndex = 1;
            this.mapPictureBox.TabStop = false;
            this.mapPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapPictureBox_MouseDown);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.closeButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.closeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.closeButton.Location = new System.Drawing.Point(292, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(26, 26);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "×";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(110)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(318, 452);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.plusOneButton);
            this.Controls.Add(this.plusTenButton);
            this.Controls.Add(this.resetTimerButton);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.distancesCheckBox);
            this.Controls.Add(this.clearAnnotationsButton);
            this.Controls.Add(this.savedLabel);
            this.Controls.Add(this.saveMapButton);
            this.Controls.Add(this.dataLabel);
            this.Controls.Add(this.calibrateButton);
            this.Controls.Add(this.mapPictureBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(318, 452);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dungeons";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MapPictureBox mapPictureBox;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.Button saveMapButton;
        private System.Windows.Forms.Label savedLabel;
        private System.Windows.Forms.Timer saveLabelHideTimer;
        private System.Windows.Forms.Button clearAnnotationsButton;
        private System.Windows.Forms.CheckBox distancesCheckBox;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Button resetTimerButton;
        private System.Windows.Forms.Button plusTenButton;
        private System.Windows.Forms.Button plusOneButton;
        private System.Windows.Forms.Button closeButton;
    }
}

