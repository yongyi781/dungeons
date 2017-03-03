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
            this.findMapButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.statusLabel = new System.Windows.Forms.Label();
            this.dataLabel = new System.Windows.Forms.Label();
            this.saveMapButton = new System.Windows.Forms.Button();
            this.mapPictureBox = new System.Windows.Forms.PictureBox();
            this.savedLabel = new System.Windows.Forms.Label();
            this.saveLabelHideTimer = new System.Windows.Forms.Timer(this.components);
            this.pauseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // findMapButton
            // 
            this.findMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.findMapButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.findMapButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.findMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findMapButton.Location = new System.Drawing.Point(12, 12);
            this.findMapButton.Name = "findMapButton";
            this.findMapButton.Size = new System.Drawing.Size(75, 25);
            this.findMapButton.TabIndex = 2;
            this.findMapButton.Text = "&Find Map";
            this.findMapButton.UseVisualStyleBackColor = false;
            this.findMapButton.Click += new System.EventHandler(this.findMapButton_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 40);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(237, 15);
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "Please open your map, then click Find Map.";
            // 
            // dataLabel
            // 
            this.dataLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataLabel.AutoSize = true;
            this.dataLabel.Location = new System.Drawing.Point(12, 381);
            this.dataLabel.Name = "dataLabel";
            this.dataLabel.Size = new System.Drawing.Size(43, 15);
            this.dataLabel.TabIndex = 4;
            this.dataLabel.Text = "Mouse";
            // 
            // saveMapButton
            // 
            this.saveMapButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.saveMapButton.Enabled = false;
            this.saveMapButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.saveMapButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.saveMapButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveMapButton.Location = new System.Drawing.Point(174, 12);
            this.saveMapButton.Name = "saveMapButton";
            this.saveMapButton.Size = new System.Drawing.Size(75, 25);
            this.saveMapButton.TabIndex = 5;
            this.saveMapButton.Text = "&Save Map";
            this.saveMapButton.UseVisualStyleBackColor = false;
            this.saveMapButton.Click += new System.EventHandler(this.saveMapButton_Click);
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mapPictureBox.Location = new System.Drawing.Point(12, 58);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.Size = new System.Drawing.Size(318, 310);
            this.mapPictureBox.TabIndex = 1;
            this.mapPictureBox.TabStop = false;
            this.mapPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapPictureBox_MouseMove);
            // 
            // savedLabel
            // 
            this.savedLabel.AutoSize = true;
            this.savedLabel.ForeColor = System.Drawing.Color.Lime;
            this.savedLabel.Location = new System.Drawing.Point(255, 17);
            this.savedLabel.Name = "savedLabel";
            this.savedLabel.Size = new System.Drawing.Size(41, 15);
            this.savedLabel.TabIndex = 6;
            this.savedLabel.Text = "Saved!";
            this.savedLabel.Visible = false;
            // 
            // saveLabelHideTimer
            // 
            this.saveLabelHideTimer.Interval = 1500;
            this.saveLabelHideTimer.Tick += new System.EventHandler(this.saveLabelHideTimer_Tick);
            // 
            // pauseButton
            // 
            this.pauseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(181)))), ((int)(((byte)(153)))));
            this.pauseButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(106)))), ((int)(((byte)(89)))));
            this.pauseButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(136)))), ((int)(((byte)(114)))));
            this.pauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pauseButton.ForeColor = System.Drawing.Color.Maroon;
            this.pauseButton.Location = new System.Drawing.Point(93, 12);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 25);
            this.pauseButton.TabIndex = 7;
            this.pauseButton.Text = "&Pause";
            this.pauseButton.UseVisualStyleBackColor = false;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(110)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(342, 405);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.savedLabel);
            this.Controls.Add(this.saveMapButton);
            this.Controls.Add(this.dataLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.findMapButton);
            this.Controls.Add(this.mapPictureBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(358, 444);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dungeons";
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox mapPictureBox;
        private System.Windows.Forms.Button findMapButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label dataLabel;
        private System.Windows.Forms.Button saveMapButton;
        private System.Windows.Forms.Label savedLabel;
        private System.Windows.Forms.Timer saveLabelHideTimer;
        private System.Windows.Forms.Button pauseButton;
    }
}

